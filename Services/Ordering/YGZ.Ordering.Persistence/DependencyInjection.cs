

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YGZ.Ordering.Application.Core.Abstractions.Data;
using YGZ.Ordering.Persistence.Data;
using YGZ.Ordering.Persistence.Data.Interceptors;

namespace YGZ.Ordering.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistenceLayer(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString(ConnectionStrings.OrderingDb)!;

        services.AddHealthChecks()
            .AddNpgSql(connectionString);

        services.AddSingleton(new ConnectionStrings(connectionString));

        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();
        services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

        services.AddDbContext<ApplicationDbContext>((serviceProvider ,options) =>
        {
            //AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            serviceProvider.GetServices<ISaveChangesInterceptor>();

            options.UseNpgsql(connectionString);
        });

        return services;
    }
}
