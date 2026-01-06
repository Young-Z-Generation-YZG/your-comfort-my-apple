using System.Reflection;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using YGZ.Basket.Infrastructure.Settings;
using YGZ.BuildingBlocks.Shared.Abstractions.HttpContext;
using YGZ.BuildingBlocks.Shared.Behaviors;
using YGZ.BuildingBlocks.Shared.Errors;
using YGZ.BuildingBlocks.Shared.Extensions;
using YGZ.BuildingBlocks.Shared.Implementations.HttpContext;

namespace YGZ.Basket.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentationLayer(this IServiceCollection services, WebApplicationBuilder builder)
    {
        services.AddApiVersioningExtension();

        services.AddSharedExtensions(Assembly.GetExecutingAssembly());

        services.AddGlobalExceptionHandler();

        services.AddHttpContextAccessor();

        services.AddScoped<IUserHttpContext, UserHttpContext>();
        services.AddScoped<ITenantHttpContext, TenantHttpContext>();

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
                        .ConfigureResource(resource => resource.AddService("YGZ.Basket.Api"))
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
