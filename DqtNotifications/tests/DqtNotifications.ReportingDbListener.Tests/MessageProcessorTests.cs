using System.Data.SqlClient;
using DqtNotifications.ReportingDbListener.Messages;
using DqtNotifications.ReportingDbListener.Sql;
using Microsoft.Extensions.Logging.Abstractions;

namespace DqtNotifications.ReportingDbListener.Tests;

public class MessageProcessorTests : IDisposable
{
    private readonly DbFixture _fixture;
    private readonly SqlConnection _connection;
    private readonly MessageProcessor _messageProcessor;

    public MessageProcessorTests(DbFixture fixture)
    {
        _fixture = fixture;

        _connection = _fixture.GetConnection();
        var clock = new TestClock();
        var mapper = new MessageEntityMapper();
        var messageTypeRegistry = MessageTypeRegistry.DiscoverMessageTypes();
        var dbBuilder = new DbBuilder(_connection, clock, new NullLogger<DbBuilder>());
        _messageProcessor = new MessageProcessor(mapper, messageTypeRegistry, dbBuilder);
    }

    public void Dispose()
    {
        _connection?.Dispose();
    }

    [Theory]
    [InlineData("SampleMessages/Create.json")]
    [InlineData("SampleMessages/Update.json")]
    //[InlineData("SampleMessages/Delete.json")]
    public async Task ProcessMessage_UpdatesDbSuccessfully(string messagePath)
    {
        var messageJson = File.ReadAllText(messagePath);
        await _messageProcessor.ProcessMessage(messageJson, CancellationToken.None);
    }
}
