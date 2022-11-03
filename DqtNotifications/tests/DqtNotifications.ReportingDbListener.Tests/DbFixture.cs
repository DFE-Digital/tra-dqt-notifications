using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Respawn;

namespace DqtNotifications.ReportingDbListener.Tests;

public class DbFixture : IAsyncLifetime
{
    private Respawner _respawner = default!;

    public DbFixture(TestConfiguration testConfiguration)
    {
        ConnectionString = testConfiguration.Configuration.GetConnectionString("DefaultConnection");
    }

    public string ConnectionString { get; }

    public Task ClearData() => _respawner.ResetAsync(ConnectionString);

    public Task DisposeAsync() => Task.CompletedTask;

    public async Task<SqlConnection> GetConnection()
    {
        var conn = new SqlConnection(ConnectionString);
        await conn.OpenAsync();
        return conn;
    }

    public async Task InitializeAsync()
    {
        _respawner = await Respawner.CreateAsync(ConnectionString);
        await ResetSchema();
    }

    public async Task<IReadOnlyCollection<IReadOnlyDictionary<string, object?>>> Query(
        string sql,
        object parameters)
    {
        using var conn = await GetConnection();

        return (await conn.QueryAsync(sql, parameters))
            .Cast<IReadOnlyDictionary<string, object?>>()
            .ToArray();
    }

    public async Task ResetSchema()
    {
        // Super temporary hacky way to create a basic schema for testing

        var sql = @"
if exists (select 1 from sysobjects where type = 'U' and name = 'contact')
    drop table contact

create table contact(
    Id uniqueidentifier primary key not null,
    SinkCreatedOn datetime,
    SinkModifiedOn datetime,
    versionnumber bigint,
    firstname nvarchar(200),
    middlename nvarchar(200),
    lastname nvarchar(200))
";

        using var conn = await GetConnection();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = sql;
        await cmd.ExecuteNonQueryAsync();
    }
}
