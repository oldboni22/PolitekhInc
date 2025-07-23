using Microsoft.Extensions.Configuration;

namespace RabbitMqController.Abstractions;

public interface IRabbitMqContext
{
    IConfiguration Configuration { get; }
}