using System.Reflection;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;
using Quartz;
using YGZ.BuildingBlocks.Shared.Extensions;
using YGZ.Catalog.Api.Protos;

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

        var catalogServiceAddress = configuration["GrpcSettings:CatalogUrl"]!;

        services.AddGrpcClient<CatalogProtoService.CatalogProtoServiceClient>(options =>
        {
            options.Address = new Uri(catalogServiceAddress);
        })
        .ConfigurePrimaryHttpMessageHandler(() =>
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };

            return handler;
        });

        return services;
    }
}
