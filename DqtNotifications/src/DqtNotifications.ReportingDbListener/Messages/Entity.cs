namespace DqtNotifications.ReportingDbListener.Messages;

public class Entity
{
    public Guid Id { get; set; }
    public string LogicalName { get; set; } = default!;
    public long RowVersion { get; init; }
    public IReadOnlyDictionary<string, object?> Attributes { get; set; } = default!;
    public IReadOnlyDictionary<string, string> FormattedValues { get; set; } = default!;
}
