using System.Reflection;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using YGZ.BuildingBlocks.Shared.Behaviors;
using YGZ.BuildingBlocks.Shared.Extensions;
using YGZ.Discount.Infrastructure.Settings;

namespace YGZ.Discount.Grpc;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentationLayer(this IServiceCollection services, WebApplicationBuilder builder)
    {
        services.AddSharedExtensions(Assembly.GetExecutingAssembly());

        AddTracingAndLogging(builder);

        return services;
    }

    public static IServiceCollection AddTracingAndLogging(WebApplicationBuilder builder)
    {
        builder.Host.AddSerilogExtension(builder.Configuration);

        builder.Services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);

            config.AddOpenBehavior(typeof(RequestLoggingPipelineBehavior<,>));
        });

        var otelSettings = new OpenTelemetrySettings();
        builder.Configuration.GetSection(OpenTelemetrySettings.SettingKey).Bind(otelSettings);

        builder.Services.AddOpenTelemetry()
                        .ConfigureResource(resource => resource.AddService("YGZ.Discount.Grpc"))
                        .WithTracing(tracing =>
                        {
                            tracing.AddHttpClientInstrumentation()
                                   .AddAspNetCoreInstrumentation();

                            tracing.AddOtlpExporter(options =>
                            {
                                options.Endpoint = new Uri(otelSettings.OtelExporterOtlpEndpointJaeger);
                                options.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
                            });

                            tracing.AddOtlpExporter(options =>
                            {
                                options.Endpoint = new Uri(otelSettings.OtelExporterOtlpEndpointSeq);
                                options.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.HttpProtobuf;
                            });
                        });

        return builder.Services;
    }
}
