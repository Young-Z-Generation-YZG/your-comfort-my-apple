

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace YGZ.Identity.Infrastructure.Extensions;

public static class IdentityServerExtension
{
    public static IServiceCollection AddIdentityServerExtension(this IServiceCollection services, IConfiguration configuration)
    {

        return services;
    }
}
