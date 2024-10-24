
using FluentValidation;
using MediatR;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;
using YGZ.Catalog.Application.Common.Behaviors;

namespace YGZ.Catalog.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services, IConfiguration configuration)
    {
        var assembly = Assembly.GetExecutingAssembly();

        services.AddSwaggerExamplesFromAssemblies(assembly);
        services.AddFluentValidationRulesToSwagger();

        services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(assembly));

        //services.AddScoped(typeof(IPipelineBehavior<,>), typeof(RequestLoggingPipelineBehavior<,>));

        // Registering the ValidationPipelineBehavior
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));
        services.AddValidatorsFromAssembly(assembly);
        return services;
    }
}
