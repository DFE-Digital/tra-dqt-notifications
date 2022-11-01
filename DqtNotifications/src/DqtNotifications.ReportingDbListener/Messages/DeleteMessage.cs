using Newtonsoft.Json;

namespace DqtNotifications.ReportingDbListener.Messages;

[Message("Delete")]
public class DeleteMessage : IMessage
{
    public IReadOnlyDictionary<string, object> InputParameters { get; set; } = default!;

    [JsonIgnore]
    public EntityReference Target => (EntityReference)InputParameters["Target"];
}
