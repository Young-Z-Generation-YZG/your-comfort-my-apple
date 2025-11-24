using System.Reflection;
using YGZ.BuildingBlocks.Shared.Abstractions.HttpContext;
using YGZ.BuildingBlocks.Shared.Errors;
using YGZ.BuildingBlocks.Shared.Extensions;
using YGZ.BuildingBlocks.Shared.Implementations.HttpContext;
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

        services.AddSignalR();

        services.AddScoped<IUserHttpContext, UserHttpContext>();
        services.AddScoped<ITenantHttpContext, TenantHttpContext>();

        return services;
    }

    public static IEndpointRouteBuilder MapGrpcEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGrpcService<OrderingRpcService>();

        return endpoints;
    }
}
