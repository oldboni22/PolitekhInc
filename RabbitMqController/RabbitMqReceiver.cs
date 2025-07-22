using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMqController.Abstractions;

namespace RabbitMqController;

public class RabbitMqReceiver(ILogger? logger = null) : IRabbitMqReceiver
{
    private IConnection? _connection;
    private IChannel? _channel;
    private readonly ILogger? _logger = logger;

    private AsyncEventingBasicConsumer? _consumer;

    public async Task InitializeAsync(string hostName)
    {
        var factory = new ConnectionFactory
        {
            HostName = hostName
        };
        _connection = await factory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();
        await _channel.QueueDeclareAsync(durable: true, queue: "Test_1", exclusive: false);

        _consumer = new AsyncEventingBasicConsumer(_channel);
        _consumer.ReceivedAsync += (_,__) =>
        {
            _logger?.LogInformation("A test message was received");
            return Task.CompletedTask;
        };

    await _channel.BasicConsumeAsync("Test_1", autoAck: true, consumer: _consumer);
    }

    public void SignForTest(AsyncEventHandler<BasicDeliverEventArgs> handler)
    {
        if (_consumer == null)
        {
            _logger?.LogWarning("The message receiver is uninitialized!");
            return;
        }
        _consumer.ReceivedAsync += handler;
    }

    public async ValueTask DisposeAsync()
    {
        if (_channel != null)
        {
            await _channel.CloseAsync();
            await _channel.DisposeAsync();
        }
        if (_connection != null)
        {
            await _connection.CloseAsync();
            await _connection.DisposeAsync();
        }
    }
}