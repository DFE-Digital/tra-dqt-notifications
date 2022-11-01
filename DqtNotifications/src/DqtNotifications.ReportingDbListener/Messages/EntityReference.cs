namespace DqtNotifications.ReportingDbListener.Messages;

public class EntityReference
{
    public Guid Id { get; set; }
    public string LogicalName { get; set; } = default!;
}
