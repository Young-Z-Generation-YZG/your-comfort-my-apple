
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using YGZ.Identity.Domain.Users;
using IdentityDbContext = YGZ.Identity.Infrastructure.Persistence.IdentityDbContext;

namespace YGZ.Identity.Infrastructure.Extensions;


public static class IdentityExtension
{
    public static IServiceCollection AddIdentityExtension(this IServiceCollection services)
    {
        services
            .AddIdentityCore<User>(options =>
            {
                options.SignIn.RequireConfirmedEmail = true;
            })
            .AddDefaultTokenProviders()
            .AddEntityFrameworkStores<IdentityDbContext>();

        return services;
    }
}
