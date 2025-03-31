using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YGZ.BuildingBlocks.Shared.Extensions;
using YGZ.Identity.Application.Abstractions.Data;
using YGZ.Identity.Application.Abstractions.Emails;
using YGZ.Identity.Application.Abstractions.Services;
using YGZ.Identity.Application.Abstractions.Utils;
using YGZ.Identity.Infrastructure.Email;
using YGZ.Identity.Infrastructure.Email.Templates;
using YGZ.Identity.Infrastructure.Extensions;
using YGZ.Identity.Infrastructure.Persistence.Interceptors;
using YGZ.Identity.Infrastructure.Persistence.Repositories;
using YGZ.Identity.Infrastructure.Services;
using YGZ.Identity.Infrastructure.Settings;
using YGZ.Identity.Infrastructure.Utils;

namespace YGZ.Identity.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddKeycloakIdentityServerExtension(configuration);

        services.AddPostgresDatabase(configuration);

        services.AddIdentityExtension();

        services.AddKeycloakOpenTelemetryExtensions();

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString(ConnectionStrings.RedisDb);
        });

        services.Configure<KeycloakSettings>(configuration.GetSection(KeycloakSettings.SettingKey));
        services.Configure<MailSettings>(configuration.GetSection(MailSettings.SettingKey));

        services.AddSingleton(_ =>
        {
            return new List<IEmailClassifier>()
            {
                new EmailVerificationTemplate(),
            };
        });

        services.AddHttpClient<IKeycloakService, KeycloakService>();
        services.AddTransient<IIdentityService, IdentityService>();
        services.AddTransient<ICachedRepository, CachedRepository>();
        services.AddTransient<IShippingAddressRepository, ShippingAddressRepository>();
        services.AddTransient<IProfileRepository, ProfileRepository>();
        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<IEmailService, EmailService>();
        services.AddTransient<IOtpGenerator, OtpGenerator>();

        return services;
    }

    public static IServiceCollection AddPostgresDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString(ConnectionStrings.IdentityDb);

        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventInterceptor>();

        services.AddDbContext<Persistence.IdentityDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            options.UseNpgsql(connectionString);
        });

        return services;
    }
}
