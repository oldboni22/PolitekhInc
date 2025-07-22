using System.Text;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMqController.Abstractions;

namespace RabbitMqController;

public class RabbitMqSender(ILogger? logger = null) : IRabbitMqSender
{
    private IConnection? _connection;
    private IChannel? _channel;

    private readonly ILogger? _logger = logger;

    public async Task InitializeAsync(string hostName)
    {
        var factory = new ConnectionFactory
        {
            HostName = hostName
        };
        _connection = await factory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();
        await _channel.QueueDeclareAsync(durable: true, queue: "Test_1", exclusive: false);
    }
    
    public async Task SendTestAsync(string message)
    {
        if (_channel == null)
        {
            _logger?.LogWarning("The message sender is uninitialized!");
            return;
        }

        try
        {
            var body = Encoding.UTF8.GetBytes(message);
            await _channel.BasicPublishAsync(exchange: string.Empty, routingKey: "Test_1", body);
            logger?.LogInformation("A test message was sent");
        }
        catch(Exception e)
        {
            _logger?.LogError(e,"An exception occured while sending a message.");
        }
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