using System.Reflection;
using Microsoft.AspNetCore.Mvc.Razor;
using YGZ.BuildingBlocks.Shared.Errors;
using YGZ.BuildingBlocks.Shared.Extensions;

namespace YGZ.Keycloak.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentationLayer(this IServiceCollection services)
    {
        services.AddRazorPages();

        services.Configure<RazorViewEngineOptions>(options =>
        {
            options.ViewLocationFormats.Add("/Views/Emails/{0}.cshtml");
        });

        services.AddApiVersioningExtension();

        services.AddMappingExtensions(Assembly.GetExecutingAssembly());

        services.AddGlobalExceptionHandler();

        return services;
    }

    public static IServiceCollection AddMappings(this IServiceCollection services)
    {
        return services;
    }
}
