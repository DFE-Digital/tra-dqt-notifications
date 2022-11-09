using System.Collections.ObjectModel;

namespace DqtNotifications.ReportingDbListener;

public class EntityDelta
{
    private static readonly IReadOnlyDictionary<string, object?> _emptyAttributeCollection = new ReadOnlyDictionary<string, object?>(new Dictionary<string, object?>());

    private EntityDelta(string entityLogicalName, Guid id, long? version, IReadOnlyDictionary<string, object?> attributes, EntityDeltaType entityDeltaType)
    {
        EntityLogicalName = entityLogicalName;
        Id = id;
        Version = version;
        Attributes = attributes;
        EntityDeltaType = entityDeltaType;
    }

    public static EntityDelta Create(string entityLogicalName, Guid id, long version, IReadOnlyDictionary<string, object?> attributes) =>
        new(entityLogicalName, id, version, attributes, EntityDeltaType.Create);

    public static EntityDelta Delete(string entityLogicalName, Guid id) =>
        new(entityLogicalName, id, null, _emptyAttributeCollection, entityDeltaType: EntityDeltaType.Delete);

    public static EntityDelta Update(string entityLogicalName, Guid id, long version, IReadOnlyDictionary<string, object?> attributes) =>
        new(entityLogicalName, id, version, attributes, EntityDeltaType.Update);

    public string EntityLogicalName { get; }
    public Guid Id { get; }
    public long? Version { get; }
    public IReadOnlyDictionary<string, object?> Attributes { get; }
    public EntityDeltaType EntityDeltaType { get; }
}

public enum EntityDeltaType { Create, Update, Delete }
