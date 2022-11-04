using System.Collections.ObjectModel;

namespace DqtNotifications.ReportingDbListener;

public class EntityDelta
{
    private static readonly IReadOnlyDictionary<string, object?> _emptyAttributeCollection = new ReadOnlyDictionary<string, object?>(new Dictionary<string, object?>());

    public EntityDelta(string entityLogicalName, Guid id, IReadOnlyDictionary<string, object?> attributes)
        : this(entityLogicalName, id, attributes, isDelete: false)
    {
        EntityLogicalName = entityLogicalName;
        Id = id;
        Attributes = attributes;
    }

    private EntityDelta(string entityLogicalName, Guid id, IReadOnlyDictionary<string, object?> attributes, bool isDelete)
    {
        EntityLogicalName = entityLogicalName;
        Id = id;
        Attributes = attributes;
        IsDelete = isDelete;
    }

    public static EntityDelta Delete(string entityLogicalName, Guid id) =>
        new(entityLogicalName, id, _emptyAttributeCollection, isDelete: true);

    public string EntityLogicalName { get; }
    public Guid Id { get; }
    public IReadOnlyDictionary<string, object?> Attributes { get; }
    public bool IsDelete { get; }
}
