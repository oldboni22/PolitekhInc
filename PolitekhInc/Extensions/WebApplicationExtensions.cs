using System.Text;
using Microsoft.AspNetCore.Diagnostics;
using RabbitMqController;
using RabbitMqController.Abstractions;

namespace PolitekhInc.Extensions;

public static class WebApplicationExtensions
{
    public static void ConfigureExceptionHandling(this WebApplication application)
    {
        application.UseExceptionHandler(builder => builder.Run
        (
            async context =>
            {
                var exceptionFeature = context.Features.Get<IExceptionHandlerFeature>();
                if(exceptionFeature == null) 
                    return;

                context.Response.StatusCode = exceptionFeature.Error switch
                {
                    
                    _ => StatusCodes.Status500InternalServerError,
                };
                
                StringBuilder sb = new();

                sb.Append($"Response status code {context.Response.StatusCode} \n");
                sb.Append($"Error message - {exceptionFeature.Error.Message}");
                
                var message = sb.ToString();
                
                var logger = application.Services.GetService(typeof(ILogger)) as ILogger;
                logger?.LogError(message);

                await context.Response.WriteAsync(message);

            })
        );
    }

    public static async Task InitializeRabbitMq(this WebApplication application,string hostName)
    {
        var sender = application.Services.GetService<IRabbitMqSender>() as RabbitMqSender;
        await sender!.InitializeAsync(hostName);
        var receiver = application.Services.GetService<IRabbitMqReceiver>() as RabbitMqReceiver;
        await receiver!.InitializeAsync(hostName);
    }
    
}