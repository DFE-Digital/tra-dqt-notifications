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
        await ClearData();
    }
}
