
using FluentValidation;
using GYZ.Discount.Grpc;
using MediatR;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using YGZ.Basket.Application.Common.Behaviors;
using YGZ.Catalog.Api.Protos;

namespace YGZ.Basket.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services, IConfiguration configuration)
    {
        var assembly = Assembly.GetExecutingAssembly();

        //services.AddSwaggerExamplesFromAssemblies(assembly);
        services.AddFluentValidationRulesToSwagger();

        services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(assembly));

        services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(options =>
        {
            options.Address = new Uri(configuration["GrpcSettings:DiscountUrl"]!);
        })
        .ConfigurePrimaryHttpMessageHandler(serviceProvider =>
        {
            return new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };
        });

        services.AddGrpcClient<CatalogProtoService.CatalogProtoServiceClient>(options =>
        {
            options.Address = new Uri(configuration["GrpcSettings:CatalogUrl"]!);
        })
        .ConfigurePrimaryHttpMessageHandler(serviceProvider =>
        {
            return new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };
        });

        //services.AddScoped(typeof(IPipelineBehavior<,>), typeof(RequestLoggingPipelineBehavior<,>));

        // Registering the ValidationPipelineBehavior
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));
        services.AddValidatorsFromAssembly(assembly);
        return services;
    }
}

