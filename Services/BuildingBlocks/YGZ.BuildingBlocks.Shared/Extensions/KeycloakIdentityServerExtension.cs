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

        // Add custom claims transformation to extract roles from Keycloak JWT
        // services.AddScoped<IClaimsTransformation, KeycloakClaimsTransformation>();

        services
            .AddAuthorization(o =>
            {
                o.AddPolicy(Policies.REQUIRE_AUTHENTICATION, b =>
                {
                    b.RequireResourceRoles(Roles.USER);
                });
            })
            .AddKeycloakAuthorization(configuration)
            .AddAuthorizationServer(configuration);

        return services;
    }
}

// public class KeycloakClaimsTransformation : IClaimsTransformation
// {
//     public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
//     {
//         var identity = principal.Identity as ClaimsIdentity;
//         if (identity == null) return Task.FromResult(principal);

//         // Extract roles from resource_access section
//         var resourceAccessClaim = identity.FindFirst("resource_access");
//         if (resourceAccessClaim != null)
//         {
//             try
//             {
//                 var resourceAccess = JsonSerializer.Deserialize<JsonElement>(resourceAccessClaim.Value);

//                 // Look for the specific client roles
//                 if (resourceAccess.TryGetProperty("ybstore-admin-dashboard-client-id", out var clientRoles))
//                 {
//                     if (clientRoles.TryGetProperty("roles", out var roles))
//                     {
//                         foreach (var role in roles.EnumerateArray())
//                         {
//                             var roleValue = role.GetString();
//                             if (!string.IsNullOrEmpty(roleValue))
//                             {
//                                 identity.AddClaim(new Claim(ClaimTypes.Role, roleValue));
//                             }
//                         }
//                     }
//                 }
//             }
//             catch (JsonException)
//             {
//                 // If JSON parsing fails, continue without adding roles
//             }
//         }

//         return Task.FromResult(principal);
//     }
// }
