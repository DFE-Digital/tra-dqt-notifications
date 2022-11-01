using Newtonsoft.Json;

namespace DqtNotifications.ReportingDbListener.Serialization;

public static class SerializerSettings
{
    public static JsonSerializerSettings CreateSettings(MessageTypeRegistry messageTypeRegistry)
    {
        var serializerSettings = new JsonSerializerSettings();

        serializerSettings.Converters.Add(new KeyValuePairCollectionJsonConverter());
        serializerSettings.Converters.Add(new MessageJsonConverter(messageTypeRegistry));
        serializerSettings.Converters.Add(new CrmObjectJsonConverter());

        return serializerSettings;
    }
}
