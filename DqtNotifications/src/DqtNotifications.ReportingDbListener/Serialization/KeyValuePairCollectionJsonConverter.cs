using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DqtNotifications.ReportingDbListener.Serialization;

public class KeyValuePairCollectionJsonConverter : JsonConverter<IReadOnlyDictionary<string, object>>
{
    public override IReadOnlyDictionary<string, object> ReadJson(
        JsonReader reader,
        Type objectType,
        IReadOnlyDictionary<string, object> existingValue,
        bool hasExistingValue,
        JsonSerializer serializer)
    {
        var result = new Dictionary<string, object>();

        var jArray = JArray.Load(reader);

        foreach (var item in jArray)
        {
            var entryObj = (JObject)item;
            var key = entryObj.GetValue("key").ToObject<string>();
            var value = entryObj.GetValue("value").ToObject<object>(serializer);
            result.Add(key, value);
        }

        return result;
    }

    public override void WriteJson(JsonWriter writer, IReadOnlyDictionary<string, object> value, JsonSerializer serializer)
    {
        throw new NotSupportedException();
    }
}
