namespace DqtNotifications.ReportingDbListener.Messages;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class MessageAttribute : Attribute
{
    public MessageAttribute(string messageName)
    {
        MessageName = messageName;
    }

    public string MessageName { get; }
}
