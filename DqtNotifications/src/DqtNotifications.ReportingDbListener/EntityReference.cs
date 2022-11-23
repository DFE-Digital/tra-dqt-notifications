namespace DqtNotifications.ReportingDbListener;

public class EntityReference
{
    public EntityReference(string entityLogicalName, Guid id)
    {
        EntityLogicalName = entityLogicalName;
        Id = id;
    }

    public string EntityLogicalName { get; }

    public Guid Id { get; }
}
