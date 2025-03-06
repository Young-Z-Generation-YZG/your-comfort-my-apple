using System.Reflection;
using YGZ.BuildingBlocks.Shared.Errors;
using YGZ.BuildingBlocks.Shared.Extensions;
using YGZ.Catalog.Api.HttpContext;
using YGZ.Catalog.Application.Abstractions;

namespace YGZ.Catalog.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentationLayer(this IServiceCollection services)
    {
        services.AddApiVersioningExtension();

        services.AddMappingExtensions(Assembly.GetExecutingAssembly());

        services.AddGlobalExceptionHandler();

        services.AddHttpContextAccessor();

        services.AddScoped<IUserContext, UserContext>();

        return services;
    }
}
