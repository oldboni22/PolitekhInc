using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMqController.Abstractions;

namespace RabbitMqController;

public class RabbitMqSender(IRabbitMqContext context, ILogger? logger = null) : IRabbitMqSender
{
    private IConnection? _connection;
    private IChannel? _channel;

    private readonly ILogger? _logger = logger;
    private readonly IConfiguration _configuration = context.Configuration;
    
    private string? _exchangeName;
    
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

        _exchangeName ??= GetExchangeName(_configuration);
        
        await _channel.ExchangeDeclareAsync(
            exchange: _exchangeName,
            type: ExchangeType.Direct,
            durable: true);
    }
    
    public async Task SendTestAsync(string message, string route)
    {
        if (_channel == null || _channel.IsClosed)
        {
            _logger?.LogWarning("The message sender is uninitialized!");
            return;
        }
        try
        {
            var body = Encoding.UTF8.GetBytes(message);
            await _channel.BasicPublishAsync(exchange: _exchangeName!, routingKey: route, body);
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