using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;
using YGZ.BuildingBlocks.Shared.Extensions;
using YGZ.Catalog.Api.Protos;

namespace YGZ.Discount.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services, IConfiguration configuration)
    {
        var assembly = Assembly.GetExecutingAssembly();

        // Add MediatR
        services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(assembly));

        services.AddSharedExtensions(assembly);


        // Add Feature Management 
        services.AddFeatureManagement();

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
