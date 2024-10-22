
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YGZ.Identity.Domain.Core.Configs;
using YGZ.Identity.Domain.Identity.Entities;
using YGZ.Identity.Persistence.Infrastructure;

namespace YGZ.Identity.Persistence.Extensions;

public static class IdentityServerExtension
{
    public static IServiceCollection AddIdentityServerExtension(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString(ConnectionString.SettingKey)!;

        var settings =  configuration.GetSection(nameof(AppConfig)).Get<AppConfig>()!;

        var migrationAssembly = typeof(IdentityServerExtension).Assembly.FullName;

        Console.WriteLine("settings" + settings);

        // Console.WriteLine("IssuerUri" + settings.IdentityServerConfig.IssuerUrl);

        services.AddIdentityServer(options =>
        {
            options.Events.RaiseErrorEvents = true;
            options.Events.RaiseInformationEvents = true;
            options.Events.RaiseFailureEvents = true;
            options.Events.RaiseSuccessEvents = true;

            options.IssuerUri = "https://localhost:8081";
            options.EmitStaticAudienceClaim = true;
        })
        .AddConfigurationStore(options =>
        {
            options.ConfigureDbContext = context => 
                context.UseNpgsql(
                        connectionString,
                        sql => sql.MigrationsAssembly(migrationAssembly)
                );
        })
        .AddOperationalStore(options =>
        {
            options.ConfigureDbContext = context =>
                context.UseNpgsql(
                    connectionString,
                    sql => sql.MigrationsAssembly(migrationAssembly)
                );

            options.EnableTokenCleanup = true;
        })
        .AddAspNetIdentity<User>();

        return services;
    }
}
