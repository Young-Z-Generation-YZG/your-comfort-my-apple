

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YGZ.Ordering.Persistence.Data;

namespace YGZ.Ordering.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistenceLayer(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString(ConnectionStrings.OrderingDb)!;

        services.AddSingleton(new ConnectionStrings(connectionString));


        services.AddDbContext<ApplicationDbContext>(options =>
        {
            //AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            options.UseNpgsql(connectionString);
        });

        return services;
    }
}
