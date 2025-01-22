using System.Reflection;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using YGZ.BuildingBlocks.Shared.Errors;
using YGZ.BuildingBlocks.Shared.Extensions;
using YGZ.IdentityServer.Infrastructure.Settings;

namespace YGZ.Identity.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentationLayer(this IServiceCollection services, IConfiguration configuration)
    {
        var identityServerSettings = configuration.GetSection(IdentityServerSettings.SettingKey!).Get<IdentityServerSettings>()!;

        services.AddMappings();

        services.AddSwaggerExtension(Assembly.GetExecutingAssembly(), identityServerSettings.KeycloakSettings.AuthrozationUrl);

        services.AddApiVersioningExtension();

        services.AddGlobalExceptionHandler();

        return services;
    }

    public static IServiceCollection AddMappings(this IServiceCollection services)
    {
        var config = TypeAdapterConfig.GlobalSettings;

        config.Scan(Assembly.GetExecutingAssembly());

        services.AddSingleton(config);

        services.AddScoped<IMapper, ServiceMapper>();

        return services;
    }

    public static IServiceCollection AddGlobalExceptionHandler(this IServiceCollection services)
    {
        services.AddSingleton<ProblemDetailsFactory, ApplicationProblemDetailsFactory>();

        return services;
    }
}
