using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace BuildingBlocks.Telemetry;

public static class OTelExtension
{
    public static IServiceCollection AddOTelService(this IServiceCollection services, string resourceName)
    {
        services
            .AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService(resourceName))
            .WithMetrics(metrics =>
            {
                metrics
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddOtlpExporter();
            })
            .WithTracing(tracing =>
            {
                tracing
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddOtlpExporter();
            });

        return services;
    }

    public static ILoggingBuilder AddOTelProvider(this ILoggingBuilder logging)
    {
        logging.AddOpenTelemetry(option => 
            option.AddOtlpExporter());

        return logging;
    }
}
