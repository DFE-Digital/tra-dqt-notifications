using System.Collections.Immutable;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Microsoft.Extensions.Logging;

namespace DqtNotifications.ReportingDbListener.Sql;

public class DbBuilder : IDbBuilder
{
    private const string VersionNumberColumnName = "versionnumber";

    private readonly SqlConnection _connection;
    private readonly IClock _clock;
    private readonly ILogger<DbBuilder> _logger;

    public DbBuilder(
        SqlConnection connection,
        IClock clock,
        ILogger<DbBuilder> logger)
    {
        _connection = connection;
        _clock = clock;
        _logger = logger;
    }

    public static ImmutableDictionary<string, object?> MapAttributesToColumns(EntityDelta entity)
    {
        return entity.Attributes
            .SelectMany(attr =>
            {
                var columns = new List<(string ColumnName, object? Value)>();

                if (attr.Value is EntityReference entityReference)
                {
                    // EntityReference attributes get two columns; one for the ID and one for the entity type
                    columns.Add((attr.Key, entityReference.Id));
                    columns.Add(($"{attr.Key}_entitytype", entityReference.EntityLogicalName));
                }
                else
                {
                    columns.Add((attr.Key, attr.Value));
                }

                return columns;
            })
            .Append((ColumnName: VersionNumberColumnName, Value: (object?)entity.Version))
            .ToImmutableDictionary(t => t.ColumnName, t => t.Value);
    }

    public async Task<ApplyEntityDeltaResult> ApplyEntityDelta(
        EntityDelta entity,
        CancellationToken cancellationToken)
    {
        if (entity.EntityDeltaType == EntityDeltaType.Delete)
        {
            var deleteResult = await DeleteEntityRow(entity.EntityLogicalName, entity.Id, cancellationToken);

            return deleteResult switch
            {
                DeleteEntityRowResult.Success => ApplyEntityDeltaResult.Success,
                DeleteEntityRowResult.RowDoesNotExist => ApplyEntityDeltaResult.StaleDataError,
                _ => throw new NotSupportedException($"Unrecognised {nameof(DeleteEntityRowResult)}: '{deleteResult}'.")
            };
        }
        else
        {
            var columns = MapAttributesToColumns(entity);
            var rowStateHint = entity.EntityDeltaType == EntityDeltaType.Create ? RowStateHint.NewRow : RowStateHint.ExistingRow;
            var upsertResult = await UpsertEntityRow(entity.EntityLogicalName, entity.Id, rowStateHint, columns, cancellationToken);

            return upsertResult switch
            {
                UpsertEntityRowResult.Success => ApplyEntityDeltaResult.Success,
                UpsertEntityRowResult.StaleDataError => ApplyEntityDeltaResult.StaleDataError,
                UpsertEntityRowResult.RowIsDeleted => ApplyEntityDeltaResult.Success,
                _ => throw new NotSupportedException($"Unrecognised {nameof(UpsertEntityRowResult)}: '{upsertResult}'.")
            };
        }
    }

    public virtual async Task<DeleteEntityRowResult> DeleteEntityRow(
        string tableName,
        Guid id,
        CancellationToken cancellationToken)
    {
        _logger.LogDebug("Deleting row for {Id} in {TableName}", id, tableName);

        var entityLogicalName = tableName;

        await EnsureConnectionOpen(cancellationToken);

        using var cmd = GetDeleteStatement();
        cmd.Connection = _connection;

        using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
        await reader.ReadAsync(cancellationToken);
        var result = (DeleteEntityRowResult)reader.GetInt32(0);

        if (result == DeleteEntityRowResult.Success)
        {
            _logger.LogDebug("Delete succeeded for {Id} in {TableName}", id, tableName);
        }
        else
        {
            _logger.LogDebug("Row does not exist for {Id} in {TableName}", id, tableName);
        }

        return result;

        SqlCommand GetDeleteStatement()
        {
            var commandTextBuilder = new StringBuilder();
            commandTextBuilder.AppendLine($"DECLARE @Deleted TABLE ({VersionNumberColumnName} bigint not null)");
            commandTextBuilder.Append("DELETE FROM ");
            commandTextBuilder.Append(QuoteName(tableName));
            commandTextBuilder.Append($" OUTPUT deleted.{VersionNumberColumnName} INTO @Deleted");
            commandTextBuilder.AppendLine(" WHERE [Id] = @Id");
            commandTextBuilder.AppendLine();
            commandTextBuilder.AppendLine("IF @@ROWCOUNT = 1");
            commandTextBuilder.AppendLine("BEGIN");
            commandTextBuilder.AppendLine("  INSERT INTO DeleteLog ([EntityName], [RecordId], [SinkDeleteTime], [VersionNumber])");
            commandTextBuilder.AppendLine($"  SELECT @EntityLogicalName, @Id, @Now, {VersionNumberColumnName} FROM @Deleted");
            commandTextBuilder.AppendLine();
            commandTextBuilder.AppendLine("  SELECT 0 Result");
            commandTextBuilder.AppendLine("END");
            commandTextBuilder.AppendLine("ELSE");
            commandTextBuilder.AppendLine("  SELECT CASE WHEN COUNT(*) > 0 THEN 0 ELSE 1 END AS Result FROM DeleteLog");
            commandTextBuilder.AppendLine("  WHERE [EntityName] = @EntityLogicalName AND [RecordId] = @Id");

            var cmd = new SqlCommand(commandTextBuilder.ToString());
            cmd.Parameters.Add(new SqlParameter("@Id", id));
            cmd.Parameters.Add(new SqlParameter("@Now", _clock.UtcNow));
            cmd.Parameters.Add(new SqlParameter("@EntityLogicalName", entityLogicalName));

            return cmd;
        }
    }

    public virtual async Task<UpsertEntityRowResult> UpsertEntityRow(
        string tableName,
        Guid id,
        RowStateHint rowStateHint,
        ImmutableDictionary<string, object?> columnValues,
        CancellationToken cancellationToken)
    {
        if (!columnValues.ContainsKey(VersionNumberColumnName))
        {
            throw new ArgumentException($"A {VersionNumberColumnName} column is required.", nameof(columnValues));
        }

        if (columnValues["versionnumber"] is not long versionNumber)
        {
            throw new ArgumentException($"The {VersionNumberColumnName} column must be a {typeof(long).Name}.", nameof(columnValues));
        }

        var columnValuesWithoutVersionNumber = columnValues.Remove(VersionNumberColumnName);

        if (rowStateHint == RowStateHint.NewRow)
        {
            _logger.LogDebug("Inserting row for {Id} in {TableName}", id, tableName);
            var insertResult = await _InsertRow();

            if (insertResult == InsertRowResult.Success)
            {
                _logger.LogDebug("Insert succeeded for {Id} in {TableName}", id, tableName);
                return UpsertEntityRowResult.Success;
            }
            else if (insertResult == InsertRowResult.RowAlreadyExists)
            {
                _logger.LogDebug("Row already exists for {Id} in {TableName}, updating", id, tableName);
                var updateResult = await _UpdateRow();

                if (updateResult == UpdateRowResult.Success)
                {
                    _logger.LogDebug("Update succeeded for {Id} in {TableName}", id, tableName);
                    return UpsertEntityRowResult.Success;
                }
                else if (updateResult == UpdateRowResult.RowVersionStale || updateResult == UpdateRowResult.RowDoesNotExist)
                {
                    _logger.LogDebug("Out of order data detected for {Id} in {TableName}", id, tableName);
                    return UpsertEntityRowResult.StaleDataError;
                }
                else
                {
                    throw new NotSupportedException($"Unrecognised {nameof(UpsertEntityRowResult)}: '{updateResult}'.");
                }
            }
            else
            {
                throw new NotSupportedException($"Unrecognised {nameof(InsertRowResult)}: '{insertResult}'.");
            }
        }
        else if (rowStateHint == RowStateHint.ExistingRow)
        {
            _logger.LogDebug("Updating row for {Id} in {TableName}", id, tableName);
            var updateResult = await _UpdateRow();

            if (updateResult == UpdateRowResult.Success)
            {
                _logger.LogDebug("Update succeeded for {Id} in {TableName}", id, tableName);
                return UpsertEntityRowResult.Success;
            }
            else if (updateResult == UpdateRowResult.RowVersionStale || updateResult == UpdateRowResult.RowDoesNotExist)
            {
                _logger.LogDebug("Out of order data detected for {Id} in {TableName}", id, tableName);
                return UpsertEntityRowResult.StaleDataError;
            }
            else if (updateResult == UpdateRowResult.RowIsDeleted)
            {
                _logger.LogDebug("Skipping row update for deleted row {Id} in {TableName}", id, tableName);
                return UpsertEntityRowResult.RowIsDeleted;
            }
            else
            {
                throw new NotSupportedException($"Unrecognised {nameof(UpsertEntityRowResult)}: '{updateResult}'.");
            }
        }
        else
        {
            throw new NotSupportedException($"Unrecognised {nameof(RowStateHint)}: '{rowStateHint}'.");
        }

        Task<InsertRowResult> _InsertRow() => InsertRow(tableName, id, versionNumber, columnValuesWithoutVersionNumber, cancellationToken);

        Task<UpdateRowResult> _UpdateRow() => UpdateRow(tableName, id, versionNumber, columnValuesWithoutVersionNumber, cancellationToken);
    }

    public virtual async Task<InsertRowResult> InsertRow(
        string tableName,
        Guid id,
        long versionNumber,
        ImmutableDictionary<string, object?> columnValues,
        CancellationToken cancellationToken)
    {
        await EnsureConnectionOpen(cancellationToken);

        using var cmd = GetInsertStatement();
        cmd.Connection = _connection;

        try
        {
            await cmd.ExecuteNonQueryAsync(cancellationToken);
            return InsertRowResult.Success;
        }
        catch (SqlException ex) when (ex.Number == 2627)  // PK violation
        {
            return InsertRowResult.RowAlreadyExists;
        }

        SqlCommand GetInsertStatement()
        {
            var allColumns = columnValues
                .Add("Id", id)
                .Add("SinkCreatedOn", _clock.UtcNow)
                .Add("SinkModifiedOn", _clock.UtcNow)
                .Add(VersionNumberColumnName, versionNumber);

            var insertColumnsAndParameters = allColumns
                .Select((kvp, i) => (Parameter: CreateParameter(name: $"@P{i + 1}", kvp.Value), ColumnName: kvp.Key));

            var columnNameList = string.Join(", ", insertColumnsAndParameters.Select(t => QuoteName(t.ColumnName)));
            var columnParameterList = string.Join(", ", insertColumnsAndParameters.Select(p => p.Parameter.ParameterName));

            var commandTextBuilder = new StringBuilder();
            commandTextBuilder.Append("INSERT INTO ");
            commandTextBuilder.Append(QuoteName(tableName));
            commandTextBuilder.Append(" (");
            commandTextBuilder.Append(columnNameList);
            commandTextBuilder.Append(") VALUES (");
            commandTextBuilder.Append(columnParameterList);
            commandTextBuilder.Append(')');

            var cmd = new SqlCommand(commandTextBuilder.ToString());

            foreach (var p in insertColumnsAndParameters)
            {
                cmd.Parameters.Add(p.Parameter);
            }

            return cmd;
        }
    }

    public virtual async Task<UpdateRowResult> UpdateRow(
        string tableName,
        Guid id,
        long versionNumber,
        ImmutableDictionary<string, object?> columnValues,
        CancellationToken cancellationToken)
    {
        var entityLogicalName = tableName;

        await EnsureConnectionOpen(cancellationToken);

        using var cmd = GetUpdateStatement();
        cmd.Connection = _connection;

        using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
        await reader.ReadAsync(cancellationToken);
        return (UpdateRowResult)reader.GetInt32(0);

        SqlCommand GetUpdateStatement()
        {
            var allColumns = columnValues
                .Add("SinkModifiedOn", _clock.UtcNow);

            var versionNumberParameter = CreateParameter("@VersionNumber", versionNumber);
            var updateColumnsAndParameters = allColumns
                .Select((kvp, i) => (Parameter: CreateParameter(name: $"@P{i + 1}", kvp.Value), ColumnName: kvp.Key))
                .Append((Parameter: versionNumberParameter, ColumnName: VersionNumberColumnName));

            var columnSetList = string.Join(",\n", updateColumnsAndParameters.Select(p => $"{QuoteName(p.ColumnName)} = {p.Parameter.ParameterName}"));

            var commandTextBuilder = new StringBuilder();
            commandTextBuilder.Append("UPDATE ");
            commandTextBuilder.Append(QuoteName(tableName));
            commandTextBuilder.AppendLine(" SET");
            commandTextBuilder.AppendLine(columnSetList);
            commandTextBuilder.Append("WHERE Id = @Id AND ");
            commandTextBuilder.Append(QuoteName(VersionNumberColumnName));
            commandTextBuilder.AppendLine(" <= @VersionNumber");
            commandTextBuilder.AppendLine();
            commandTextBuilder.AppendLine("IF @@ROWCOUNT = 1");
            commandTextBuilder.AppendLine("  SELECT 0 AS Result");
            commandTextBuilder.AppendLine("ELSE");
            commandTextBuilder.AppendLine("  BEGIN");
            commandTextBuilder.Append("    IF EXISTS (SELECT TOP 1 1 FROM ");
            commandTextBuilder.Append(QuoteName(tableName));
            commandTextBuilder.AppendLine(" WHERE Id = @Id)");
            commandTextBuilder.AppendLine("      SELECT 2 AS Result");
            commandTextBuilder.AppendLine("    ELSE");
            commandTextBuilder.AppendLine("      SELECT CASE WHEN COUNT(*) > 0 THEN 3 ELSE 1 END AS Result FROM DeleteLog");
            commandTextBuilder.AppendLine("      WHERE [EntityName] = @EntityLogicalName AND [RecordId] = @Id");
            commandTextBuilder.Append("  END");

            var cmd = new SqlCommand(commandTextBuilder.ToString());
            cmd.Parameters.Add(CreateParameter("@Id", id));
            cmd.Parameters.Add(CreateParameter("@EntityLogicalName", entityLogicalName));

            foreach (var p in updateColumnsAndParameters)
            {
                cmd.Parameters.Add(p.Parameter);
            }

            return cmd;
        }
    }

    private static SqlParameter CreateParameter(string name, object? value)
    {
        value ??= DBNull.Value;
        return new SqlParameter(name, value);
    }

    // This is imperfect but good enough for the names we get
    private static string QuoteName(string name) => $"[{name}]";

    private async Task EnsureConnectionOpen(CancellationToken cancellationToken)
    {
        if (_connection.State == ConnectionState.Closed)
        {
            await _connection.OpenAsync(cancellationToken);
        }
    }

    public enum InsertRowResult
    {
        Success = 0,
        RowAlreadyExists = 1
    }

    public enum UpdateRowResult
    {
        Success = 0,
        RowDoesNotExist = 1,
        RowVersionStale = 2,
        RowIsDeleted = 3
    }

    public enum UpsertEntityRowResult
    {
        Success = 0,
        StaleDataError = 1,
        RowIsDeleted = 2
    }

    public enum DeleteEntityRowResult
    {
        Success = 0,
        RowDoesNotExist = 1
    }
}
