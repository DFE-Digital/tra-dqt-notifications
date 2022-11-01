namespace DqtNotifications.ReportingDbListener.Messages;

public class Entity
{
    public Guid Id { get; set; }
    public string LogicalName { get; set; } = default!;
    public string RowVersion { get; set; } = default!;
    public IReadOnlyDictionary<string, object> Attributes { get; set; } = default!;
}
