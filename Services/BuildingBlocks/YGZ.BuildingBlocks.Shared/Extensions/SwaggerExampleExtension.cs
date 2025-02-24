
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Filters;

namespace YGZ.BuildingBlocks.Shared.Extensions;

public static class SwaggerExampleExtension
{
    public static IServiceCollection AddSwaggerExampleExtension(this IServiceCollection services, Assembly assembly)
    {
        services.AddSwaggerGen(options =>
        {
            options.ExampleFilters();
        });

        services.AddSwaggerExamplesFromAssemblies(assembly);

        return services;
    }
}
