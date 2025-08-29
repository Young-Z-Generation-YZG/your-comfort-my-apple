
using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using static YGZ.BuildingBlocks.Shared.Constants.AuthorizationConstants;

namespace YGZ.BuildingBlocks.Shared.Extensions;

public static class KeycloakIdentityServerExtension
{
    public static IServiceCollection AddKeycloakIdentityServerExtension(this IServiceCollection services, IConfiguration configuration)
    {
        services
           .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
           .AddKeycloakWebApi(configuration);

        services
            .AddAuthorization(o =>
            {
                o.AddPolicy(Policies.RequireClientRole, b =>
                {
                    b.RequireResourceRoles("USER", "ADMIN");
                });
            })
            .AddKeycloakAuthorization(configuration)
            .AddAuthorizationServer(configuration);

        return services;
    }
}
