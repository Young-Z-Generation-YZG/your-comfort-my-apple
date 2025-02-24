

using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace YGZ.BuildingBlocks.Shared.Errors;

public static class GlobalExceptionHandler
{
    public static IServiceCollection AddGlobalExceptionHandler(this IServiceCollection services)
    {
        services.AddSingleton<ProblemDetailsFactory, ApplicationProblemDetailsFactory>();

        return services;
    }
}
