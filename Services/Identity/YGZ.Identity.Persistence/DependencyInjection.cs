

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YGZ.Identity.Application.Core.Abstractions.Identity;
using YGZ.Identity.Application.Core.Abstractions.TokenService;
using YGZ.Identity.Domain.Core.Configs;
using YGZ.Identity.Persistence.Extensions;
using YGZ.Identity.Persistence.Infrastructure;
using YGZ.Identity.Persistence.Services.Identity;
using YGZ.Identity.Persistence.Services.Token;

namespace YGZ.Identity.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistenceLayer(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString(ConnectionString.SettingKey)!;

        services.AddSingleton(new ConnectionString(connectionString));

        services.Configure<AppConfig>(configuration.GetSection(nameof(AppConfig)));

        services.AddIdentityExtension(configuration);

        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<ITokenService, TokenService>();

        return services;
    }
}
