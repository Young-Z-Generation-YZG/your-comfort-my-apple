using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YGZ.BuildingBlocks.Shared.Extensions;
using YGZ.Catalog.Api.Protos;
using YGZ.Discount.Grpc.Protos;

namespace YGZ.Basket.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services, IConfiguration configuration)
    {
        var assembly = Assembly.GetExecutingAssembly();

        // Add MediatR
        services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(assembly));

        // Add Fluent Validation
        services.AddFluentValidationExtension(assembly);

        var discountServiceAddress = configuration["GrpcSettings:DiscountUrl"]!;

        services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(options =>
        {
            options.Address = new Uri(discountServiceAddress);
        })
        .ConfigurePrimaryHttpMessageHandler(() =>
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };

            return handler;
        });

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
