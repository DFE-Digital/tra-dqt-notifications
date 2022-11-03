namespace DqtNotifications.ReportingDbListener.Tests;

public class TestClock : IClock
{
    // Arbitrary timestamp that can be round-tripped to/from MSSQL and doesn't change unless explicitly set
    public DateTime UtcNow { get; set; } = new(2022, 11, 2, 19, 53, 17);
}
