using DqtNotifications.ReportingDbListener.Messages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DqtNotifications.ReportingDbListener.Serialization;

public class MessageJsonConverter : JsonConverter
{
    private readonly MessageTypeRegistry _messageTypeRegistry;

    public MessageJsonConverter(MessageTypeRegistry messageTypeRegistry)
    {
        _messageTypeRegistry = messageTypeRegistry;
    }

    public override bool CanConvert(Type objectType) => objectType == typeof(IMessage);

    public override bool CanRead => true;

    public override bool CanWrite => false;

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        var messageJObject = JToken.ReadFrom(reader);
        var messageName = messageJObject["MessageName"]?.Value<string>();

        if (messageName is null)
        {
            throw new JsonException("Could not read message name.");
        }

        var messageType = _messageTypeRegistry.GetMessageType(messageName);

        if (messageType is null)
        {
            throw new JsonException($"Unrecognised message: '{messageName}'.");
        }

        return messageJObject.ToObject(messageType, serializer);
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        throw new NotSupportedException();
    }
}
