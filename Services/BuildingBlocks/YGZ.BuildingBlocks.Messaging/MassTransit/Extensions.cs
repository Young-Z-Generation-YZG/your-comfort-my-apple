using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace YGZ.BuildingBlocks.Messaging.MassTransit;

public static class Extensions
{
    public static IServiceCollection AddMessageBrokerExtensions(this IServiceCollection services, IConfiguration configuration, Assembly? assembly = null)
    {
        services.AddMassTransit(config =>
        {
            config.SetKebabCaseEndpointNameFormatter();

            if(assembly != null)
            {
                config.AddConsumers(assembly);
            }

            config.UsingRabbitMq((context, configuarator) =>
            {
                MessageBrokerSettings settings = context.GetRequiredService<IOptions<MessageBrokerSettings>>().Value;

                configuarator.Host(new Uri(settings.Host!), host =>
                {
                    host.Username(settings.Username);
                    host.Password(settings.Password);
                });

                configuarator.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}
