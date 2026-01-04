using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;
using YGZ.BuildingBlocks.Shared.Extensions;
using YGZ.Discount.Grpc.Protos;
using YGZ.Ordering.Api.Protos;
using System.Net.Http;
using Grpc.Net.Client;

namespace YGZ.Catalog.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services, IConfiguration configuration)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var discountServiceAddress = configuration["GrpcSettings:DiscountUrl"]!;
        var orderingServiceAddress = configuration["GrpcSettings:OrderingUrl"]!;

        // Add MediatR
        services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(assembly));

        // Add Fluent Validation
        services.AddFluentValidationExtension(assembly);

        services.AddSharedExtensions(Assembly.GetExecutingAssembly());


        // Add Feature Management 
        services.AddFeatureManagement();


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

        services.AddGrpcClient<OrderingProtoService.OrderingProtoServiceClient>(options =>
        {
            options.Address = new Uri(orderingServiceAddress);
        })
        .ConfigurePrimaryHttpMessageHandler(() =>
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };

            return handler;
        });
        // .ConfigureChannel(grpcChannelOptions =>
        // {
        //     // Configure HTTP/2 with prior knowledge for plain HTTP connections
        //     grpcChannelOptions.HttpVersion = new Version(2, 0);
        // });

        return services;
    }
}
