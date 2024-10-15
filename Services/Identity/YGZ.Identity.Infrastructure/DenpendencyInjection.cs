

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YGZ.Identity.Application.Core.Abstractions.Emails;
using YGZ.Identity.Infrastructure.Emails.Services;
using YGZ.Identity.Infrastructure.Emails.Settings;
using YGZ.Identity.Infrastructure.Emails.Templates;

namespace YGZ.Identity.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEmailService(configuration);

        return services;
    }

    public static IServiceCollection AddEmailService(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MailSettings>(configuration.GetSection(MailSettings.SettingKey));

        services.AddSingleton(_ =>
        {
            return new List<IEmailClassifier>()
            {
                new EmailVerificationTemplate(),
                new ResetPasswordTemplate(),
            };
        });


        services.AddScoped<IEmailService, EmailService>();

        services.AddTransient<IEmailNotificationService, EmailConfirmationService>();

        return services;
    }
}
