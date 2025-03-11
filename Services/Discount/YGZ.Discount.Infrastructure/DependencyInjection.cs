using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YGZ.Discount.Application.Abstractions;
using YGZ.Discount.Infrastructure.Utils;

namespace YGZ.Discount.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddSingleton<IUniqueCodeGenerator, UniqueCodeGenerator>();

        return services;
    }
}
