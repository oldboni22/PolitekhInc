using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMqController.Abstractions;

namespace RabbitMqController;

public class RabbitMqReceiver(IRabbitMqContext context, ILogger? logger = null) : IRabbitMqReceiver
{
    private IConnection? _connection;
    private IChannel? _channel;
    
    
    private readonly ILogger? _logger = logger;
    private readonly IConfiguration _configuration = context.Configuration;
    
    
    private AsyncEventingBasicConsumer? _consumer;


    private string GetQueue(IConfiguration configuration)
    {
        return configuration.
            GetSection("DirectNames").
            GetSection("Test").
            GetSection("Queue1Name").Value!;
    }
    private string GetRoute(IConfiguration configuration)
    {
        return configuration.
            GetSection("DirectNames").
            GetSection("Test").
            GetSection("Route1Name").Value!;
    }
    private string GetExchangeName(IConfiguration configuration)
    {
        return configuration.
            GetSection("DirectNames").
            GetSection("Test").
            GetSection("ExchangeName").Value!;
    }
    public async Task InitializeAsync()
    {
        if(_connection != null && _connection.IsOpen)
        {
            _logger?.LogInformation("Connection is already initialized. Initialization is skipped.");
            return;
        }
        
        var factory = new ConnectionFactory
        {
            VirtualHost = _configuration.GetSection("VirtualHost").Value!
        };
        
        _connection = await factory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();

        var queueName = GetQueue(_configuration);
        
        await _channel.QueueDeclareAsync(queue:queueName,durable: true, exclusive: false,autoDelete: false);

        await _channel.QueueBindAsync(
            queue: queueName,
            exchange: GetExchangeName(_configuration),
            routingKey:GetRoute(_configuration));

        _consumer = new AsyncEventingBasicConsumer(_channel);
        _consumer.ReceivedAsync += (_,__) =>
        {
            _logger?.LogInformation("A test message was received");
            return Task.CompletedTask;
        };

    await _channel.BasicConsumeAsync(queueName, autoAck: true, consumer: _consumer);
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