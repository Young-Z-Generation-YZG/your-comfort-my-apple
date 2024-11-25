
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Reflection;
using YGZ.Catalog.Application.Core.Abstractions.EventBus;
using YGZ.Catalog.Application.Core.Abstractions.Uploading;
using YGZ.Catalog.Domain.Core.Abstractions.Common;
using YGZ.Catalog.Infrastructure.Common;
using YGZ.Catalog.Infrastructure.MessageBroker;
using YGZ.Catalog.Infrastructure.Uploading;

namespace YGZ.Catalog.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration, Assembly? assembly = null)
    {
        services.Configure<CloudinarySettings>(configuration.GetSection(CloudinarySettings.SettingKey));
        services.Configure<MessageBrokerSettings>(configuration.GetSection(MessageBrokerSettings.SettingKey));

        services.AddScoped<IUploadService, UploadService>();

        services.AddMassTransit(config =>
        {
            config.SetKebabCaseEndpointNameFormatter();

            if (assembly != null)
            {
                config.AddConsumers(assembly);
            }

            config.UsingRabbitMq((context, configurator) =>
            {
                MessageBrokerSettings settings = context.GetRequiredService<IOptions<MessageBrokerSettings>>().Value;

                configurator.Host(new Uri(configuration[settings.Host]!), host =>
                {
                    host.Username(configuration[settings.Username]!);
                    host.Password(configuration[settings.Password]!);
                });

                configurator.ConfigureEndpoints(context);
            });
            //busConfiguarator.SetKebabCaseEndpointNameFormatter();

            ////busConfiguarator.AddConsumer<ProductCreatedEventComsumer>();

            //busConfiguarator.UsingRabbitMq((context, configurator) =>
            //{
            //    MessageBrokerSettings settings = context.GetRequiredService<IOptions<MessageBrokerSettings>>().Value;

            //    Console.WriteLine("settings.Host" + settings.Host);
            //    Console.WriteLine("settings.Username" + settings.Username);
            //    Console.WriteLine("settings.Password" + settings.Password);

            //    configurator.Host(new Uri(settings.Host), host =>
            //    {
            //        host.Username(settings.Username);
            //        host.Password(settings.Password);
            //    });

            //    configurator.ConfigureEndpoints(context);

            //});
        });

        services.AddTransient<IEventBus, EventBus>();
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        return services;
    }
}
