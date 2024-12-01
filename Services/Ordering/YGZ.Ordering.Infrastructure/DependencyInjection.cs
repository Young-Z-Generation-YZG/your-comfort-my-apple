using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;
using System.Reflection;
using YGZ.BuildingBlocks.Messaging.MassTransit;

namespace YGZ.Ordering.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration, Assembly? assembly = null)
    {
        services.AddFeatureManagement();

        services.AddMessageBrokerExtensions(configuration, assembly);

        //services.Configure<MessageBrokerSettings>(configuration.GetSection(MessageBrokerSettings.SettingKey));

        //services.AddMassTransit(config =>
        //{
        //    config.SetKebabCaseEndpointNameFormatter();

        //    if (assembly != null) 
        //    {
        //        config.AddConsumers(assembly);
        //    }


        //    config.UsingRabbitMq((context, configurator) =>
        //    {
        //        configurator.Host(new Uri(configuration["MessageBrokerSettings:Host"]!), host =>
        //        {
        //            host.Username(configuration["MessageBrokerSettings:UserName"]!);
        //            host.Password(configuration["MessageBrokerSettings:Password"]!);
        //        });

        //        configurator.ConfigureEndpoints(context);
        //    });

        //});


        return services;
    }
}
