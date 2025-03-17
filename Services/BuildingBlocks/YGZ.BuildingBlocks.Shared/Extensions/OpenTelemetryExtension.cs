

using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
namespace YGZ.BuildingBlocks.Shared.Extensions;

public static class OpenTelemetryExtension
{
    public static IServiceCollection AddKeycloakOpenTelemetryExtensions(this IServiceCollection services)
    {
        services
            .AddOpenTelemetry()
            .WithMetrics(metrics =>
                metrics
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddKeycloakAuthServicesInstrumentation()
            )
            .WithTracing(tracing =>
                tracing
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddKeycloakAuthServicesInstrumentation()
            )
            .UseOtlpExporter();

        return services;
    }
}
