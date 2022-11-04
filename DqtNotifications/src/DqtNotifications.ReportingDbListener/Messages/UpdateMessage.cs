using Newtonsoft.Json;

namespace DqtNotifications.ReportingDbListener.Messages;

[Message("Update")]
public class UpdateMessage : IMessage
{
    public IReadOnlyDictionary<string, object?> InputParameters { get; set; } = default!;

    [JsonIgnore]
    public Entity Target => (Entity)InputParameters["Target"]!;
}
