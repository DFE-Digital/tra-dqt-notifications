using DqtNotifications.ReportingDbListener.Messages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DqtNotifications.ReportingDbListener.Serialization;

public class CrmObjectJsonConverter : JsonConverter
{
    public override bool CanRead => true;

    public override bool CanWrite => false;

    public override bool CanConvert(Type typeToConvert) => typeToConvert == typeof(object);

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        var jToken = JToken.ReadFrom(reader);

        if (jToken.Type != JTokenType.Object)
        {
            return ReadDefault();
        }

        var type = jToken["__type"]?.ToString();

        if (type is null)
        {
            return ReadDefault();
        }
        else if (type == "Entity:http://schemas.microsoft.com/xrm/2011/Contracts")
        {
            return jToken.ToObject<Entity>(serializer);
        }
        else if (type == "EntityReference:http://schemas.microsoft.com/xrm/2011/Contracts")
        {
            return jToken.ToObject<EntityReference>(serializer);
        }
        else if (type == "OptionSetValue:http://schemas.microsoft.com/xrm/2011/Contracts")
        {
            return jToken.ToObject<OptionSetValue>(serializer);
        }
        else
        {
            throw new NotImplementedException($"Unknown CRM type: '{type}'.");
        }

        object? ReadDefault() => jToken is JValue jValue ? jValue.Value : jToken;
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        throw new NotSupportedException();
    }
}
