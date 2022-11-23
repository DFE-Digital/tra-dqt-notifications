using System.Reflection;
using DqtNotifications.ReportingDbListener.Messages;

namespace DqtNotifications.ReportingDbListener;

public class MessageTypeRegistry
{
    private readonly Dictionary<string, Type> _messageTypesByName;

    private MessageTypeRegistry(IEnumerable<(string MessageName, Type MessageType)> messages)
    {
        _messageTypesByName = messages.ToDictionary(t => t.MessageName, t => t.MessageType, StringComparer.OrdinalIgnoreCase);
    }

    public static MessageTypeRegistry DiscoverMessageTypes()
    {
        var messagesNamespace = $"{typeof(MessageTypeRegistry).Namespace}.Messages";
        var messageInterfaceType = typeof(IMessage);

        var messageTypes = from type in Assembly.GetExecutingAssembly().GetTypes()
                           where type.Namespace == messagesNamespace && type.IsPublic && !type.IsAbstract && type.IsAssignableTo(messageInterfaceType)
                           let messageAttr = type.GetCustomAttribute<MessageAttribute>()
                           where messageAttr is not null
                           select (messageAttr.MessageName, MessageType: type);

        return new MessageTypeRegistry(messageTypes);
    }

    public Type? GetMessageType(string messageName) => _messageTypesByName.GetValueOrDefault(messageName);
}
