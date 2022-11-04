using Newtonsoft.Json;

namespace DqtNotifications.ReportingDbListener.Serialization;

public static class SerializerSettings
{
    public static JsonSerializerSettings CreateSettings(MessageTypeRegistry messageTypeRegistry)
    {
        var serializerSettings = new JsonSerializerSettings();

        serializerSettings.Converters.Add(new StringObjectKeyValuePairCollectionJsonConverter());
        serializerSettings.Converters.Add(new StringStringKeyValuePairCollectionJsonConverter());
        serializerSettings.Converters.Add(new MessageJsonConverter(messageTypeRegistry));
        serializerSettings.Converters.Add(new CrmObjectJsonConverter());

        return serializerSettings;
    }
}
