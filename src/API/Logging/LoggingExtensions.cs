using Serilog;

namespace API.Logging;

public static class LoggingExtensions
{
    public static IHostBuilder UseApiLogging(this IHostBuilder host)
    {
        return host.UseSerilog((context, services, config) => config
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services) 
            .Enrich.FromLogContext());
    }
}