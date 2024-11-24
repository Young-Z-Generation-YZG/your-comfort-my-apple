using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using YGZ.Ordering.Infrastructure.MessageBroker;
using MassTransit;

namespace YGZ.Ordering.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration, Assembly? assembly = null)
    {
        services.Configure<MessageBrokerSettings>(configuration.GetSection(MessageBrokerSettings.SettingKey));

        services.AddMassTransit(config =>
        {
            config.SetKebabCaseEndpointNameFormatter();

            if (assembly != null) 
            {
                config.AddConsumers(assembly);
            }

            Console.WriteLine("MessageBrokerSettings:Host: " + configuration["MessageBrokerSettings:Host"]);
            Console.WriteLine("MessageBrokerSettings:UserName: " + configuration["MessageBrokerSettings:UserName"]);
            Console.WriteLine("MessageBrokerSettings:Password: " + configuration["MessageBrokerSettings:Password"]);


            config.UsingRabbitMq((context, configurator) =>
            {
                configurator.Host(new Uri(configuration["MessageBrokerSettings:Host"]!), host =>
                {
                    host.Username(configuration["MessageBrokerSettings:UserName"]!);
                    host.Password(configuration["MessageBrokerSettings:Password"]!);
                });

                configurator.ConfigureEndpoints(context);
            });

        });


        return services;
    }
}
