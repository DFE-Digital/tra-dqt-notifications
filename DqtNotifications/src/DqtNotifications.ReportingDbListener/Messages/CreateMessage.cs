using Newtonsoft.Json;

namespace DqtNotifications.ReportingDbListener.Messages;

public record CreateMessage : IMessage
{
    public static string MessageType => "Create";

    public required IReadOnlyDictionary<string, object> InputParameters { get; init; }

    [JsonIgnore]
    public Entity Target => (Entity)InputParameters["Target"]!;
}
