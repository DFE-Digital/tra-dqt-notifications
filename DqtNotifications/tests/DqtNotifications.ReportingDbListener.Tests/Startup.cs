using Microsoft.Extensions.DependencyInjection;

namespace DqtNotifications.ReportingDbListener.Tests;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        var testConfiguration = new TestConfiguration();

        services.AddSingleton(testConfiguration);
        services.AddSingleton<DbFixture>();
    }
}
