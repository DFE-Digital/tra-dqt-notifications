using System.Collections.Immutable;

namespace DqtNotifications.ReportingDbListener.Sql;

public interface IDbBuilder
{
    Task<UpsertEntityRowResult> UpsertEntityRow(
        string tableName,
        Guid id,
        RowStateHint rowStateHint,
        ImmutableDictionary<string, object?> columnValues,
        CancellationToken cancellationToken);
}
