using PolitekhInc.Extensions;
using Serilog;

namespace PolitekhInc;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.ConfigureLogging();
        
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
        

        app.Run();
    }
}