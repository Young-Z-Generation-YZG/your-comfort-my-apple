using System.Reflection;
using YGZ.BuildingBlocks.Shared.Extensions;

namespace YGZ.Discount.Grpc;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentationLayer(this IServiceCollection services)
    {
        services.AddMappingExtensions(Assembly.GetExecutingAssembly());

        return services;
    }
}
