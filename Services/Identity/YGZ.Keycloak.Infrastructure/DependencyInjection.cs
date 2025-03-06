using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YGZ.Keycloak.Application.Abstractions;
using YGZ.Keycloak.Application.Abstractions.Emails;
using YGZ.Keycloak.Application.Abstractions.Mails;
using YGZ.Keycloak.Infrastructure.Extensions;
using YGZ.Keycloak.Infrastructure.Mail;
using YGZ.Keycloak.Infrastructure.Mail.Templates;
using YGZ.Keycloak.Infrastructure.Persistence;
using YGZ.Keycloak.Infrastructure.Services;
using YGZ.Keycloak.Infrastructure.Settings;

namespace YGZ.Keycloak.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddKeycloakIdentityServerExtensions(configuration);

        services.AddPostgresDatabase(configuration);

        services.AddIdentityExtension();

        services.AddOpenTelemetryExtensions();

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

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });

        return services;
    }
}
