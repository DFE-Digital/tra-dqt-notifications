namespace DqtNotifications.ReportingDbListener.Messages;

public record EntityReference
{
    public required Guid Id { get; init; }
    public required string LogicalName { get; init; }
}
