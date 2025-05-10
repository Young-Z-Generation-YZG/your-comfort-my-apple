using System.Reflection;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;
using Quartz;
using YGZ.BuildingBlocks.Shared.Extensions;

namespace YGZ.Ordering.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services, IConfiguration configuration)
    {
        var assembly = Assembly.GetExecutingAssembly();

        // Add MediatR
        services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(assembly));

        // Add Fluent Validation
        services.AddFluentValidationExtension(assembly);

        // Add Feature Management 
        services.AddFeatureManagement();

        services.AddQuartz();

        services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);


        return services;
    }
}
