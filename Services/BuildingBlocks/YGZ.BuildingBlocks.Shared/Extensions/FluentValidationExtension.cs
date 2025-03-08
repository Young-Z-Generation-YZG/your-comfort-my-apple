
using System.Reflection;
using FluentValidation;
using MediatR;
//using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using YGZ.BuildingBlocks.Shared.Behaviors;

namespace YGZ.BuildingBlocks.Shared.Extensions;

public static class FluentValidationExtension
{
    public static IServiceCollection AddFluentValidationExtension(this IServiceCollection services, Assembly assembly)
    {
        services.AddValidatorsFromAssembly(assembly);

        //services.AddFluentValidationRulesToSwagger();

        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));

        return services;
    }
}
