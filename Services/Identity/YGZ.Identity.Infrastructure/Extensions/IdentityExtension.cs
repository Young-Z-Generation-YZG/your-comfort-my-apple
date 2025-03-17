
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using YGZ.Identity.Domain.Users;
using YGZ.Identity.Infrastructure.Persistence;

namespace YGZ.Identity.Infrastructure.Extensions;


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
