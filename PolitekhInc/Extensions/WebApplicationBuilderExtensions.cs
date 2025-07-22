using Serilog;

namespace PolitekhInc.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static void ConfigureLogging(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((_, config) =>
        {
            config.WriteTo.Console();
            config.WriteTo.File("/Logs/.log");
            config.Enrich.FromLogContext();
        });
    }
}