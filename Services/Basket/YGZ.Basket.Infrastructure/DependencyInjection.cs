using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using YGZ.BuildingBlocks.Messaging.MassTransit;

namespace YGZ.Basket.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration, Assembly? assembly = null)
    {
        services.AddMessageBrokerExtensions(configuration);

        return services;
    }
}
