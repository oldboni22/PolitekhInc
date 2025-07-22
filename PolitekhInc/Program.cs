using PolitekhInc.Extensions;
using Serilog;

namespace PolitekhInc;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Configuration.AddJsonFile("appsettings.json");
        
        builder.ConfigureLogging();

        builder.Services.AddRabbitMq();
        builder.Services.AddAuthorization();
        
        builder.Services.AddOpenApi();

        var app = builder.Build();

        app.UseSerilogRequestLogging();
        app.ConfigureExceptionHandling();
        
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        await app.InitializeRabbitMq(app.Configuration.GetConnectionString("RabbitMq")!);
        await app.RunAsync();
    }
}