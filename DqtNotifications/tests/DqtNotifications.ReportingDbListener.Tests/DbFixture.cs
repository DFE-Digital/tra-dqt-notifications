using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.SqlServer.Dac;
using Respawn;

namespace DqtNotifications.ReportingDbListener.Tests;

public class DbFixture
{
    private readonly Respawner _respawner = default!;

    public DbFixture(TestConfiguration testConfiguration)
    {
        ConnectionString = testConfiguration.Configuration.GetConnectionString("DefaultConnection");

        _respawner = Task.Run(() => Respawner.CreateAsync(ConnectionString).GetAwaiter().GetResult()).Result;
        ResetSchema();
    }

    public string ConnectionString { get; }

    public Task ClearData() => _respawner.ResetAsync(ConnectionString);

    public SqlConnection GetConnection()
    {
        var conn = new SqlConnection(ConnectionString);
        conn.Open();
        return conn;
    }

    public async Task<IReadOnlyCollection<IReadOnlyDictionary<string, object?>>> Query(
        string sql,
        object parameters)
    {
        using var conn = GetConnection();

        return (await conn.QueryAsync(sql, parameters))
            .Cast<IReadOnlyDictionary<string, object?>>()
            .ToArray();
    }

    public void ResetSchema()
    {
        var dacpacLocation = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../../../src/DqtNotifications.ReportingDb/bin/",
#if DEBUG
            "Debug",
#else
            "Release",
#endif
            "DqtNotifications.ReportingDb.dacpac"));

        var dbName = new SqlConnectionStringBuilder(ConnectionString).InitialCatalog;

        using var package = DacPackage.Load(dacpacLocation);
        var result = new DacServices(ConnectionString).Publish(
            package,
            dbName,
            new PublishOptions()
            {
                DeployOptions = new DacDeployOptions()
                {
                    BlockOnPossibleDataLoss = false,
                    DropObjectsNotInSource = true,
                    DoNotDropObjectTypes = new[]
                    {
                        ObjectType.Logins,
                        ObjectType.Users,
                        ObjectType.Permissions,
                        ObjectType.RoleMembership
                    }
                }
            });
    }
}
