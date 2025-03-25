using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YGZ.Discount.Application.Abstractions;
using YGZ.Discount.Application.Abstractions.Data;
using YGZ.Discount.Domain.Abstractions.Data;
using YGZ.Discount.Infrastructure.Persistence;
using YGZ.Discount.Infrastructure.Persistence.Repositories;
using YGZ.Discount.Infrastructure.Settings;
using YGZ.Discount.Infrastructure.Utils;

namespace YGZ.Discount.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddSingleton<IUniqueCodeGenerator, UniqueCodeGenerator>();

        services.AddScoped<IDiscountRepository, DiscountRepository>();

        services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
        services.AddScoped<IPromotionCouponRepository, PromotionCouponRepository>();

        services.AddPostgresDatabase(configuration);

        return services;
    }

    public static IServiceCollection AddPostgresDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString(ConnectionStrings.DiscountDb);

        services.AddDbContext<DiscountDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });

        return services;
    }
}
