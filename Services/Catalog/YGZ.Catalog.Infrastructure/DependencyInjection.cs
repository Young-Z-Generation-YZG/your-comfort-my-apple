
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using YGZ.Catalog.Application.Core.Abstractions.EventBus;
using YGZ.Catalog.Application.Core.Abstractions.Uploading;
using YGZ.Catalog.Application.Products.Events;
using YGZ.Catalog.Domain.Core.Abstractions.Common;
using YGZ.Catalog.Infrastructure.Common;
using YGZ.Catalog.Infrastructure.MessageBroker;
using YGZ.Catalog.Infrastructure.Uploading;

namespace YGZ.Catalog.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<CloudinarySettings>(configuration.GetSection(CloudinarySettings.SettingKey));
        services.Configure<MessageBrokerSettings>(configuration.GetSection(MessageBrokerSettings.SettingKey));

        services.AddScoped<IUploadService, UploadService>();

        services.AddMassTransit(busConfiguarator =>
        {
            busConfiguarator.SetKebabCaseEndpointNameFormatter();

            //busConfiguarator.AddConsumer<ProductCreatedEventComsumer>();

            busConfiguarator.UsingRabbitMq((context, configurator) =>
            {
                MessageBrokerSettings settings = context.GetRequiredService<IOptions<MessageBrokerSettings>>().Value;

                Console.WriteLine("settings.Host" + settings.Host);
                Console.WriteLine("settings.Username" + settings.Username);
                Console.WriteLine("settings.Password" + settings.Password);

                configurator.Host(new Uri(settings.Host), host =>
                {
                    host.Username(settings.Username);
                    host.Password(settings.Password);
                });

                configurator.ConfigureEndpoints(context);

            });
        });

        services.AddTransient<IEventBus, EventBus>();
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        return services;
    }
}
