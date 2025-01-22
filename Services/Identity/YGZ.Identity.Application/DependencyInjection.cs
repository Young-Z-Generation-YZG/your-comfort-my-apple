using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YGZ.BuildingBlocks.Shared.Extensions;

namespace YGZ.Identity.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services, IConfiguration configuration)
    {
        var assembly = Assembly.GetExecutingAssembly();

        services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(assembly));

        services.AddFluentValidationExtension(assembly);

        return services;
    }
}
