using Microsoft.AspNetCore.Mvc.Razor;
using System.Reflection;
using YGZ.BuildingBlocks.Shared.Errors;
using YGZ.BuildingBlocks.Shared.Extensions;
using YGZ.Identity.Api.HttpContext;
using YGZ.Identity.Application.Abstractions.HttpContext;

namespace YGZ.Identity.Api;

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

        services.AddHttpContextAccessor();

        services.AddScoped<IUserContext, UserContext>();

        return services;
    }

    public static IServiceCollection AddMappings(this IServiceCollection services)
    {
        return services;
    }
}
