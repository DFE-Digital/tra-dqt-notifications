using DqtNotifications.ReportingDbListener.Messages;
using DqtNotifications.ReportingDbListener.Serialization;
using DqtNotifications.ReportingDbListener.Sql;
using Newtonsoft.Json;

namespace DqtNotifications.ReportingDbListener;

public class MessageProcessor
{
    private readonly MessageEntityMapper _messageEntityMapper;
    private readonly IDbBuilder _dbBuilder;
    private readonly JsonSerializerSettings _serializerSettings;

    public MessageProcessor(
        MessageEntityMapper messageEntityMapper,
        MessageTypeRegistry messageTypeRegistry,
        IDbBuilder dbBuilder)
    {
        _serializerSettings = SerializerSettings.CreateSettings(messageTypeRegistry);
        _messageEntityMapper = messageEntityMapper;
        _dbBuilder = dbBuilder;
    }

    public async Task ProcessMessage(string messageJson, CancellationToken cancellationToken)
    {
        var message = (IMessage)JsonConvert.DeserializeObject(messageJson, typeof(IMessage), _serializerSettings);
        var entity = _messageEntityMapper.MapMessage(message);

        await _dbBuilder.ApplyEntityDelta(entity, cancellationToken);
        // TODO if we get a stale data error then hit API
    }
}
