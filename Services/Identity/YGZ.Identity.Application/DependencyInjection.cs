using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using YGZ.BuildingBlocks.Shared.Extensions;

namespace YGZ.Identity.Application;


public static class DependencyInjection
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services, IConfiguration configuration)
    {
        var assembly = Assembly.GetExecutingAssembly();

        // Add MediatR
        services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(assembly));

        // Add Fluent Validation
        services.AddFluentValidationExtension(assembly);

        return services;
    }
}
