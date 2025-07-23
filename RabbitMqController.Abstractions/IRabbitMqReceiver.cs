using Microsoft.Extensions.Configuration;
using RabbitMQ.Client.Events;

namespace RabbitMqController.Abstractions;

public interface IRabbitMqReceiver : IAsyncDisposable
{
    Task InitializeAsync();
    void SignForTest(AsyncEventHandler<BasicDeliverEventArgs> handler);
}