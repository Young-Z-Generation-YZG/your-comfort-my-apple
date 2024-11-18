
using FluentValidation;
using MediatR;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using YGZ.Basket.Application.Common.Behaviors;

namespace YGZ.Basket.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services, IConfiguration configuration)
    {
        var assembly = Assembly.GetExecutingAssembly();

        //services.AddSwaggerExamplesFromAssemblies(assembly);
        services.AddFluentValidationRulesToSwagger();

        services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(assembly));

        //services.AddScoped(typeof(IPipelineBehavior<,>), typeof(RequestLoggingPipelineBehavior<,>));

        // Registering the ValidationPipelineBehavior
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));
        services.AddValidatorsFromAssembly(assembly);
        return services;
    }
}

