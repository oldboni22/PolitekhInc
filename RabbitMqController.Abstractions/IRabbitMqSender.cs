using Microsoft.Extensions.Configuration;

namespace RabbitMqController.Abstractions;

public interface IRabbitMqSender : IAsyncDisposable
{
    Task InitializeAsync();
    Task SendTestAsync(string message, string route);
}