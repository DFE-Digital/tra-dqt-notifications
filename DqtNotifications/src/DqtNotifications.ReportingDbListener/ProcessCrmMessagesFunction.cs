using Azure.Messaging.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.ServiceBus;
using Microsoft.Extensions.Logging;

namespace DqtNotifications.ReportingDbListener;

public class ProcessCrmMessagesFunction
{
    private readonly ServiceBusMessageActions _serviceBusMessageActions;

    public ProcessCrmMessagesFunction(ServiceBusMessageActions serviceBusMessageActions)
    {
        _serviceBusMessageActions = serviceBusMessageActions;
    }

    [FunctionName(nameof(ProcessCrmMessagesFunction))]
    public async Task Run(
        [ServiceBusTrigger(
            topicName: "crm-messages",
            subscriptionName: "ReportingDbListener",
            Connection = "ReportingDbListener",
            AutoCompleteMessages = false,
            IsSessionsEnabled = true)]
        ServiceBusReceivedMessage message,
        ILogger logger,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Processed message {MessageId}.", message.MessageId);
        await _serviceBusMessageActions.CompleteMessageAsync(message, cancellationToken);
    }
}
