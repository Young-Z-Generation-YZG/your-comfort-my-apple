using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace YGZ.IdentityServer.Application;

public static class DependencyInjections
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services, IConfiguration configuration)
    {
        return services;
    }
}
