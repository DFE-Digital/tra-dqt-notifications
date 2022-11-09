namespace DqtNotifications.ReportingDbListener.Messages;

public interface IMessage
{
    abstract static string MessageType { get; }
}
