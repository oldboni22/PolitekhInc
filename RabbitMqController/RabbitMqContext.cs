using Microsoft.Extensions.Configuration;
using RabbitMqController.Abstractions;

namespace RabbitMqController;

public class RabbitMqContext : IRabbitMqContext
{
    public IConfiguration Configuration { get; }

    public RabbitMqContext()
    {
        Configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("RabbitMqConfig.json", optional: false, reloadOnChange: true)
            .AddJsonFile("RabbitMqRoutes.json", optional: false, reloadOnChange: true)
            .Build();
    }

}