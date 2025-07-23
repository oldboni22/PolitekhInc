using RabbitMqController;
using RabbitMqController.Abstractions;

namespace PolitekhInc.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddRabbitMq(this IServiceCollection collection)
    {
        collection.AddSingleton<IRabbitMqContext,RabbitMqContext>();
        
        collection.AddSingleton<IRabbitMqReceiver,RabbitMqReceiver>();
        collection.AddSingleton<IRabbitMqSender,RabbitMqSender>();
    }
}