using System.Reflection;
using YGZ.BuildingBlocks.Shared.Extensions;
using YGZ.Ordering.Api.HttpContext;
using YGZ.Ordering.Application.Abstractions;
using YGZ.BuildingBlocks.Shared.Errors;
using YGZ.Ordering.Api.RpcServices;

namespace YGZ.Ordering.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentationLayer(this IServiceCollection services)
    {
        services.AddApiVersioningExtension();

        services.AddSharedExtensions(Assembly.GetExecutingAssembly());

        services.AddGlobalExceptionHandler();

        services.AddHttpContextAccessor();

        services.AddScoped<IUserRequestContext, UserRequestContext>();

        return services;
    }

    public static IEndpointRouteBuilder MapGrpcEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGrpcService<OrderingRpcService>();

        return endpoints;
    }
}
