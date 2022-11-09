using Newtonsoft.Json;

namespace DqtNotifications.ReportingDbListener.Messages;

public record DeleteMessage : IMessage
{
    public static string MessageType => "Delete";

    public required IReadOnlyDictionary<string, object> InputParameters { get; init; }

    [JsonIgnore]
    public EntityReference Target => (EntityReference)InputParameters["Target"];
}
