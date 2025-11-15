using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YGZ.BuildingBlocks.Messaging.Extensions;
using YGZ.Discount.Domain.Abstractions;
using YGZ.Discount.Domain.Abstractions.Data;
using YGZ.Discount.Infrastructure.Persistence;
using YGZ.Discount.Infrastructure.Persistence.Interceptors;
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

        services.AddQueuesFromApplicationLayer(configuration);

        var redisConnectionString = configuration.GetConnectionString(ConnectionStrings.RedisDb)!;

        services.AddSingleton<StackExchange.Redis.IConnectionMultiplexer>(sp =>
            StackExchange.Redis.ConnectionMultiplexer.Connect(redisConnectionString));

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisConnectionString;
        });

        return services;
    }

    public static IServiceCollection AddPostgresDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString(ConnectionStrings.DiscountDb);

        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventInterceptor>();

        services.AddDbContext<DiscountDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            options.UseNpgsql(connectionString);
        });

        return services;
    }

    public static IServiceCollection AddQueuesFromApplicationLayer(this IServiceCollection services, IConfiguration configuration)
    {
        var queuesFromAssembly = AppDomain.CurrentDomain
            .GetAssemblies()
            .FirstOrDefault(asm => asm.GetName().Name == "YGZ.Discount.Application");

        services.AddMessageBrokerExtensions(configuration, queuesFromAssembly);

        return services;
    }
}
