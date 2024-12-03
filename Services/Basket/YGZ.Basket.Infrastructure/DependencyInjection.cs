using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using YGZ.Basket.Application.Core.Abstractions.Payments;
using YGZ.Basket.Infrastructure.Payments.Vnpay;
using YGZ.BuildingBlocks.Messaging.MassTransit;

namespace YGZ.Basket.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration, Assembly? assembly = null)
    {
        services.AddHttpContextAccessor();

        services.Configure<VnpaySettings>(configuration.GetSection(VnpaySettings.SettingKey));

        services.AddSingleton<IVnpayPaymentProvider, VnpayPaymentProvider>();

        services.AddMessageBrokerExtensions(configuration);

        return services;
    }
}
