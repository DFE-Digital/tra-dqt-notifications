using DqtNotifications.ReportingDbListener.Messages;

namespace DqtNotifications.ReportingDbListener.Tests.Messages;

public class MessageEntityMapperTests
{
    [Fact]
    public void MapMessage_DeleteMessage_MappedToDeleteEntityDelta()
    {
        var id = Guid.NewGuid();
        var entityLogicalName = "contact";

        var message = new DeleteMessage()
        {
            InputParameters = new Dictionary<string, object>()
            {
                {
                    "Target",
                    new ReportingDbListener.Messages.EntityReference()
                    {
                        Id = id,
                        LogicalName = entityLogicalName
                    }
                }
            }
        };

        var mapper = new MessageEntityMapper();

        var result = mapper.MapMessage(message);

        Assert.Equal(id, result.Id);
        Assert.Equal(entityLogicalName, result.EntityLogicalName);
        Assert.True(result.IsDelete);
    }

    [Fact]
    public void MapMessage_CreateMessage_MappedToEntityDelta()
    {
        var id = Guid.NewGuid();
        var entityLogicalName = "contact";

        var message = new CreateMessage()
        {
            InputParameters = new Dictionary<string, object>()
            {
                {
                    "Target",
                    new Entity()
                    {
                        Id = id,
                        LogicalName = entityLogicalName,
                        RowVersion = "42",
                        Attributes = new Dictionary<string, object?>()
                        {
                            { "firstname", "Joe" },
                            { "lastname", "Bloggs" }
                        },
                        FormattedValues = new Dictionary<string, string>()
                    }
                }
            }
        };

        var mapper = new MessageEntityMapper();

        var result = mapper.MapMessage(message);

        Assert.False(result.IsDelete);
        Assert.Equal(id, result.Id);
        Assert.Equal(entityLogicalName, result.EntityLogicalName);
    }

    [Fact]
    public void MapMessage_UpdateMessage_MappedToEntityDelta()
    {
        var id = Guid.NewGuid();
        var entityLogicalName = "contact";

        var message = new UpdateMessage()
        {
            InputParameters = new Dictionary<string, object>()
            {
                {
                    "Target",
                    new Entity()
                    {
                        Id = id,
                        LogicalName = entityLogicalName,
                        RowVersion = "42",
                        Attributes = new Dictionary<string, object?>()
                        {
                            { "firstname", "Joe" },
                            { "lastname", "Bloggs" }
                        },
                        FormattedValues = new Dictionary<string, string>()
                    }
                }
            }
        };

        var mapper = new MessageEntityMapper();

        var result = mapper.MapMessage(message);

        Assert.False(result.IsDelete);
        Assert.Equal(id, result.Id);
        Assert.Equal(entityLogicalName, result.EntityLogicalName);
    }

    [Theory]
    [MemberData(nameof(DateTimeMappingData))]
    public void MapMessage_ConvertsDateValuesToDateTime(
        string serializedValue,
        string formattedValue,
        DateTime expectedDateTime)
    {
        var message = new CreateMessage()
        {
            InputParameters = new Dictionary<string, object>()
            {
                {
                    "Target",
                    new Entity()
                    {
                        Id = Guid.NewGuid(),
                        LogicalName = "contact",
                        RowVersion = "42",
                        Attributes = new Dictionary<string, object?>()
                        {
                            { "createdon", serializedValue }
                        },
                        FormattedValues = new Dictionary<string, string>()
                        {
                            { "createdon", formattedValue }
                        }
                    }
                }
            }
        };

        var mapper = new MessageEntityMapper();

        var result = mapper.MapMessage(message);

        Assert.Collection(
            result.Attributes,
            attr =>
            {
                Assert.Equal("createdon", attr.Key);
                var dt = Assert.IsType<DateTime>(attr.Value);
                Assert.Equal(expectedDateTime, dt);
                Assert.Equal(DateTimeKind.Utc, dt.Kind);
            });
    }

    [Fact]
    public void MapMessage_ConvertsEntityReference()
    {
        var ownerId = Guid.NewGuid();
        var ownerEntityLogicalName = "systemuser";

        var message = new CreateMessage()
        {
            InputParameters = new Dictionary<string, object>()
            {
                {
                    "Target",
                    new Entity()
                    {
                        Id = Guid.NewGuid(),
                        LogicalName = "contact",
                        RowVersion = "42",
                        Attributes = new Dictionary<string, object?>()
                        {
                            {
                                "ownerid",
                                new ReportingDbListener.Messages.EntityReference()
                                {
                                    Id = ownerId,
                                    LogicalName = ownerEntityLogicalName
                                }
                            }
                        },
                        FormattedValues = new Dictionary<string, string>()
                    }
                }
            }
        };

        var mapper = new MessageEntityMapper();

        var result = mapper.MapMessage(message);

        var entityRef = Assert.IsType<EntityReference>(result.Attributes["ownerid"]);
        Assert.Equal(ownerId, entityRef.Id);
        Assert.Equal(ownerEntityLogicalName, entityRef.EntityLogicalName);
    }

    public static TheoryData<string, string, DateTime> DateTimeMappingData { get; } = new()
    {
        { "/Date(1667232842000)/", "2022-10-31T16:14:02-00:00", new DateTime(2022, 10, 31, 16, 14, 2) },
        { "/Date(643420800000+0000)/", "1990-05-23T00:00:00", new DateTime(1990, 5, 23, 0, 0, 0) }
    };
}
