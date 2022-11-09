using DqtNotifications.ReportingDbListener.Messages;
using DqtNotifications.ReportingDbListener.Serialization;
using Newtonsoft.Json;

namespace DqtNotifications.ReportingDbListener.Tests;

public class SerializationTests
{
    [Fact]
    public void SerializesCreateMessageSuccessfully()
    {
        var messageTypeRegistry = MessageTypeRegistry.DiscoverMessageTypes();
        var serializerSettings = SerializerSettings.CreateSettings(messageTypeRegistry);

        var message = File.ReadAllText("SampleMessages/Create.json");

        var result = (IMessage)JsonConvert.DeserializeObject(message, typeof(IMessage), serializerSettings);
        var createMessage = Assert.IsType<CreateMessage>(result);

        var target = createMessage.Target;
        Assert.Equal("contact", target.LogicalName);
        Assert.Equal(new Guid("c34e4970-7810-4e1f-9b34-8af0187f4271"), target.Id);

        var gendercode = Assert.IsType<OptionSetValue>(target.Attributes["gendercode"]);
        Assert.Equal(2, gendercode.Value);

        var ownerid = Assert.IsType<ReportingDbListener.Messages.EntityReference>(target.Attributes["ownerid"]);
        Assert.Equal(new Guid("ab7501c2-aa4e-ec11-8c62-000d3aba6cf0"), ownerid.Id);
        Assert.Equal("systemuser", ownerid.LogicalName);
    }

    [Fact]
    public void SerializesUpdateMessageSuccessfully()
    {
        var messageTypeRegistry = MessageTypeRegistry.DiscoverMessageTypes();
        var serializerSettings = SerializerSettings.CreateSettings(messageTypeRegistry);

        var message = File.ReadAllText("SampleMessages/Update.json");

        var result = (IMessage)JsonConvert.DeserializeObject(message, typeof(IMessage), serializerSettings);
        var updateMessage = Assert.IsType<UpdateMessage>(result);

        var target = updateMessage.Target;
        Assert.Equal("contact", target.LogicalName);
        Assert.Equal(new Guid("c34e4970-7810-4e1f-9b34-8af0187f4271"), target.Id);

        var ownerid = Assert.IsType<ReportingDbListener.Messages.EntityReference>(target.Attributes["modifiedby"]);
        Assert.Equal(new Guid("ab7501c2-aa4e-ec11-8c62-000d3aba6cf0"), ownerid.Id);
        Assert.Equal("systemuser", ownerid.LogicalName);
    }

    [Fact]
    public void SerializesDeleteMessageSuccessfully()
    {
        var messageTypeRegistry = MessageTypeRegistry.DiscoverMessageTypes();
        var serializerSettings = SerializerSettings.CreateSettings(messageTypeRegistry);

        var message = File.ReadAllText("SampleMessages/Delete.json");

        var result = (IMessage)JsonConvert.DeserializeObject(message, typeof(IMessage), serializerSettings);
        var deleteMessage = Assert.IsType<DeleteMessage>(result);

        var target = deleteMessage.Target;
        Assert.Equal("contact", target.LogicalName);
        Assert.Equal(new Guid("c34e4970-7810-4e1f-9b34-8af0187f4271"), target.Id);
    }
}
