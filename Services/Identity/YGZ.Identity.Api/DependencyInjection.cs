﻿using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Reflection;
using YGZ.BuildingBlocks.Shared.Errors;
using YGZ.BuildingBlocks.Shared.Extensions;
using YGZ.Identity.Api.Extensions;
using YGZ.Identity.Api.HttpContext;
using YGZ.Identity.Application.Abstractions.HttpContext;
using YGZ.Identity.Infrastructure.Settings;

namespace YGZ.Identity.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentationLayer(this IServiceCollection services, WebApplicationBuilder builder)
    {
        services.AddSwaggerExtension();
        services.AddApiVersioningExtension();
        services.AddSharedExtensions(Assembly.GetExecutingAssembly());
        services.AddGlobalExceptionHandler();

        AddTracingAndLogging(builder);
        AddDatabaseHealthCheck(builder);

        services.AddRazorPages();

        services.Configure<RazorViewEngineOptions>(options =>
        {
            options.ViewLocationFormats.Add("/Views/Emails/{0}.cshtml");
        });

        services.AddScoped<IUserRequestContext, UserRequestContext>();

        return services;
    }

    public static IServiceCollection AddTracingAndLogging(WebApplicationBuilder builder)
    {
        builder.Host.AddSerilogExtension(builder.Configuration);

        var otelSettings = new OpenTelemetrySettings();
        builder.Configuration.GetSection(OpenTelemetrySettings.SettingKey).Bind(otelSettings);

        builder.Services.AddOpenTelemetry()
                        .ConfigureResource(resource => resource.AddService("YGZ.Identity.Api.Test"))
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

        //builder.Services
        //    .AddOpenTelemetry()
        //    //.ConfigureResource(resource => resource.AddService("YGZ.Identity.Api"))
        //    .ConfigureResource(resource => resource.AddService("YGZ.Identity.Api.Test"))
        //    .WithMetrics(metrics =>
        //    {
        //        metrics.AddAspNetCoreInstrumentation()
        //               .AddHttpClientInstrumentation();

        //        metrics.AddOtlpExporter();
        //    })
        //    .WithTracing(tracing =>
        //    {
        //        tracing.AddHttpClientInstrumentation()
        //               .AddAspNetCoreInstrumentation();

        //        tracing.AddOtlpExporter();
        //    });
        ////builder.Services.AddKeycloakOpenTelemetryExtension();
        //builder.Logging.AddOpenTelemetry(logging =>
        //{
        //    logging.AddOtlpExporter();
        //    logging.IncludeFormattedMessage = true;
        //    logging.IncludeScopes = true;
        //});

        return builder.Services;
    }

    public static IServiceCollection AddDatabaseHealthCheck(WebApplicationBuilder builder)
    {
        builder.Services.AddHealthChecks()
        .AddNpgSql(
            connectionString: builder.Configuration.GetConnectionString("IdentityDb")!,
            name: "IdentityDb",
            failureStatus: HealthStatus.Unhealthy,
            tags: new[] { "db", "postgres" })
        .AddNpgSql(
            connectionString: builder.Configuration.GetConnectionString("KeycloakDb")!,
            name: "KeycloakDb",
            failureStatus: HealthStatus.Unhealthy,
            tags: new[] { "db", "postgres" });


        return builder.Services;
    }
}
