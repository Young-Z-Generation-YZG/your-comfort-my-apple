

using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace YGZ.Keycloak.Infrastructure.Extensions;

public static class KeycloakExtensions
{
    public static IServiceCollection AddKeycloakIdentityServerExtensions(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddKeycloakWebApi(configuration);

        services
            .AddAuthorization()
            .AddKeycloakAuthorization()
            .AddAuthorizationServer(configuration);

        return services;
    }
}  
