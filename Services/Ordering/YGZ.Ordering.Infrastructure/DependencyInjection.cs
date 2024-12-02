using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;
using System.Reflection;
using YGZ.BuildingBlocks.Messaging.MassTransit;
using YGZ.Ordering.Application.Orders.Events.Integrations;

namespace YGZ.Ordering.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration, Assembly? assembly = null)
    {
        services.AddFeatureManagement();

        var queuesFromAssembly = AppDomain.CurrentDomain
            .GetAssemblies()
            .FirstOrDefault(asm => asm.GetName().Name == "YGZ.Ordering.Application");

        services.AddMessageBrokerExtensions(configuration, queuesFromAssembly);

        //services.Configure<MessageBrokerSettings>(configuration.GetSection(MessageBrokerSettings.SettingKey));

        //services.AddMassTransit(config =>
        //{
        //    config.SetKebabCaseEndpointNameFormatter();

        //    //config.AddConsumers(typeof(BasketCheckoutIntegrationEventHandler).Assembly);

        //    config.AddConsumers(applicationAssembly);

        //    config.UsingRabbitMq((context, configurator) =>
        //    {
        //        //configurator.ReceiveEndpoint("basket-checkout-queue", endpoint =>
        //        //{
        //        //    endpoint.ConfigureConsumer<BasketCheckoutIntegrationEventHandler>(context);
        //        //});

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
