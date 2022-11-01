using Newtonsoft.Json;

namespace DqtNotifications.ReportingDbListener.Messages;

[Message("Create")]
public class CreateMessage : IMessage
{
    public IReadOnlyDictionary<string, object> InputParameters { get; set; } = default!;

    [JsonIgnore]
    public Entity Target => (Entity)InputParameters["Target"];
}
