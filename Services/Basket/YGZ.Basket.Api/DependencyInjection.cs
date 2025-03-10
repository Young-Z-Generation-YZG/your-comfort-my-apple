using System.Reflection;
using YGZ.Basket.Api.HttpContext;
using YGZ.Basket.Application.Abstractions;
using YGZ.BuildingBlocks.Shared.Errors;
using YGZ.BuildingBlocks.Shared.Extensions;

namespace YGZ.Basket.Api;

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
