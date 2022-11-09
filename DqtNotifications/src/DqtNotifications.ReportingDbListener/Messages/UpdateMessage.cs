using Newtonsoft.Json;

namespace DqtNotifications.ReportingDbListener.Messages;

public record UpdateMessage : IMessage
{
    public static string MessageType => "Update";

    public required IReadOnlyDictionary<string, object> InputParameters { get; init; }

    [JsonIgnore]
    public Entity Target => (Entity)InputParameters["Target"]!;
}
