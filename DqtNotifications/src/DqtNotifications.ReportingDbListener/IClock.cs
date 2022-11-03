namespace DqtNotifications.ReportingDbListener;

public interface IClock
{
    DateTime UtcNow { get; }
}
