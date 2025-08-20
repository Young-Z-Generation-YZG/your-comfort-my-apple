using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Reflection;
using YGZ.BuildingBlocks.Shared.Errors;
using YGZ.BuildingBlocks.Shared.Extensions;
using YGZ.Identity.Api.Extensions;
using YGZ.Identity.Api.HttpContext;
using YGZ.Identity.Application.Abstractions.HttpContext;

namespace YGZ.Identity.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentationLayer(this IServiceCollection services, WebApplicationBuilder builder)
    {
        services.AddRazorPages();

        services.Configure<RazorViewEngineOptions>(options =>
        {
            options.ViewLocationFormats.Add("/Views/Emails/{0}.cshtml");
        });

        services.AddSwaggerExtension();
        services.AddApiVersioningExtension();
        services.AddSharedExtensions(Assembly.GetExecutingAssembly());
        AddMonitoringAndLogging(services, builder);

        services.AddGlobalExceptionHandler();

        services.AddScoped<IUserRequestContext, UserRequestContext>();

        return services;
    }

    public static IServiceCollection AddMonitoringAndLogging(IServiceCollection services, WebApplicationBuilder builder)
    {
        builder.Host.AddSerilogExtension(builder.Configuration);

        // Add OpenTelemetry Logging
        builder.Logging.AddOpenTelemetry(logging =>
        {
            logging.IncludeFormattedMessage = true;
            logging.IncludeScopes = true;
        });

        services.AddHealthChecks()
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


        return services;
    }
}
