
using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace YGZ.BuildingBlocks.Shared.Extensions;

public static class KeycloakIdentityServerExtension
{
    public static IServiceCollection AddKeycloakIdentityServerExtension(this IServiceCollection services, IConfiguration configuration)
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
