

using Microsoft.Extensions.DependencyInjection;
using YGZ.Keycloak.Domain.Users;
using YGZ.Keycloak.Infrastructure.Persistence;

namespace YGZ.Keycloak.Infrastructure.Extensions;

public static class IdentityExtension
{
    public static IServiceCollection AddIdentityExtension(this IServiceCollection services)
    {
        services
            .AddIdentityCore<User>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

        return services;
    }
}
