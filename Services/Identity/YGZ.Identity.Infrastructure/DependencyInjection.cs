using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YGZ.BuildingBlocks.Shared.Extensions;
using YGZ.Identity.Application.Abstractions.Emails;
using YGZ.Identity.Application.Abstractions.Services;
using YGZ.Identity.Infrastructure.Email;
using YGZ.Identity.Infrastructure.Email.Templates;
using YGZ.Identity.Infrastructure.Extensions;
using YGZ.Identity.Infrastructure.Services;
using YGZ.Identity.Infrastructure.Settings;

namespace YGZ.Identity.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddKeycloakIdentityServerExtension(configuration);

        services.AddPostgresDatabase(configuration);

        services.AddIdentityExtension();

        services.AddKeycloakOpenTelemetryExtensions();

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
        services.AddTransient<IEmailService, EmailService>();
        //services.AddTransient<IEmailNotificationService, EmailNotificationService>();

        return services;
    }

    public static IServiceCollection AddPostgresDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString(ConnectionStrings.IdentityDb);

        services.AddDbContext<Persistence.IdentityDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });

        return services;
    }
}
