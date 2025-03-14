
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace YGZ.BuildingBlocks.Messaging.Extensions;

public static class Extensions
{
    public static IServiceCollection AddMessageBrokerExtensions(this IServiceCollection services, IConfiguration configuration, Assembly? assembly = null)
    {
        services.AddMassTransit(config =>
        {
            config.SetKebabCaseEndpointNameFormatter();

            if (assembly != null)
            {
                config.AddConsumers(assembly);
            }

            config.UsingRabbitMq((context, configuarator) =>
            {
                configuarator.Host(new Uri(configuration["MessageBrokerSettings:Host"]!), host =>
                {
                    host.Username(configuration["MessageBrokerSettings:Username"]!);
                    host.Password(configuration["MessageBrokerSettings:Password"]!);
                });

                configuarator.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}
