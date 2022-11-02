using Microsoft.Extensions.Configuration;

namespace DqtNotifications.ReportingDbListener.Tests;

public class TestConfiguration
{
    public IConfiguration Configuration { get; } =
        new ConfigurationBuilder()
            .AddUserSecrets<TestConfiguration>()
            .AddEnvironmentVariables()
            .Build();
}
