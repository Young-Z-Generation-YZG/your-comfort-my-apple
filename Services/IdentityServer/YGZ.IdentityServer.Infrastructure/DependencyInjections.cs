using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YGZ.IdentityServer.Infrastructure.Extensions.IdentityServer;
using YGZ.IdentityServer.Infrastructure.Mails;
using YGZ.IdentityServer.Infrastructure.Persistence.Data;
using YGZ.IdentityServer.Infrastructure.Settings;

namespace YGZ.IdentityServer.Infrastructure;

public static class DependencyInjections
{
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionStrings = configuration.GetConnectionString(ConnectionStrings.IdentityDb!);

        services.AddOptions();

        services.Configure<MailSettings>(configuration.GetSection(MailSettings.SettingKey!));

        services.AddSingleton<IEmailSender, SendMailService>();

        // Add ApplicationDbContext
        services.AddIdentityExtension(configuration);

        //services.AddDbContext<ApplicationDbContext>(options =>
        //{
        //    options.UseNpgsql(connectionStrings);
        //});


        return services;
    }
}
