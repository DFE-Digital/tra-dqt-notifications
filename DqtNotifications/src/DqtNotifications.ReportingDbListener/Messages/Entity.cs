namespace DqtNotifications.ReportingDbListener.Messages;

public record Entity
{
    public required Guid Id { get; init; }
    public required string LogicalName { get; init; }
    public required long RowVersion { get; init; }
    public required IReadOnlyDictionary<string, object?> Attributes { get; init; }
    public required IReadOnlyDictionary<string, string> FormattedValues { get; init; }
}
