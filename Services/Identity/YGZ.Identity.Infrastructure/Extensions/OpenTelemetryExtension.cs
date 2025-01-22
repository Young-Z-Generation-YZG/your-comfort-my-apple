

using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace YGZ.Identity.Infrastructure.Extensions;

public static class OpenTelemetryExtension
{
    public static IServiceCollection AddOpenTelemetryExtension(this IServiceCollection services)
    {
        services
            .AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService("YGZ.Identity.Api"))
            .WithTracing(tracing =>
            {
                tracing
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation();

                tracing.AddOtlpExporter();
            });

        return services;
    }
}
