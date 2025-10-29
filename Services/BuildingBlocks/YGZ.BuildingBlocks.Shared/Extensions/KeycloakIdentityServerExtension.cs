using System.Text.Json;
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

        // Optional: Enable claims transformation to map Keycloak resource_access roles 
        // to standard ASP.NET Core role claims. This allows using RequireRole() instead of RequireResourceRoles()
        // Uncomment the line below to enable:
        // services.AddScoped<IClaimsTransformation, KeycloakClaimsTransformation>();

        services
            .AddAuthorization(o =>
            {
                // Policy 1: Require authenticated user only
                o.AddPolicy(Policies.REQUIRE_AUTHENTICATION, b =>
                {
                    b.RequireAuthenticatedUser();
                });

                // Policy 2: Require ADMIN_SUPER role from Keycloak resource_access
                // Uses RequireResourceRoles - looks for roles in JWT's resource_access claim
                // This is Keycloak-specific and reads from resource_access["client-id"].roles
                o.AddPolicy(Policies.R__ADMIN_SUPER___RS__ALL, b =>
                {
                    b.RequireResourceRoles(Roles.ADMIN_SUPER);
                });

                // Alternative: If you enable the KeycloakClaimsTransformation below,
                // you can use standard RequireRole instead:
                // o.AddPolicy(Policies.R__ADMIN_SUPER___RS__ALL, b =>
                // {
                //     b.RequireRole(Roles.ADMIN_SUPER);
                // });

                // Example: Require multiple roles (AND - user must have ALL roles)
                // o.AddPolicy("RequireAdminAndManager", b =>
                // {
                //     b.RequireResourceRoles(Roles.ADMIN_SUPER, Roles.STAFF);
                // });

                // Example: Require any one of multiple roles (OR - user must have at least ONE)
                // Use RequireAssertion for OR logic:
                // o.AddPolicy("RequireAdminOrStaff", b =>
                // {
                //     b.RequireAssertion(context =>
                //     {
                //         var resourceAccessClaim = context.User.FindFirst("resource_access")?.Value;
                //         if (string.IsNullOrEmpty(resourceAccessClaim)) return false;
                //         
                //         using var jsonDoc = JsonDocument.Parse(resourceAccessClaim);
                //         var root = jsonDoc.RootElement;
                //         
                //         // Get client roles from resource_access
                //         if (root.TryGetProperty("ybstore-admin-dashboard-client-id", out var client) &&
                //             client.TryGetProperty("roles", out var roles))
                //         {
                //             var roleList = roles.EnumerateArray()
                //                 .Select(r => r.GetString())
                //                 .Where(r => !string.IsNullOrEmpty(r))
                //                 .ToList();
                //             
                //             return roleList.Contains(Roles.ADMIN_SUPER) || roleList.Contains(Roles.STAFF);
                //         }
                //         
                //         return false;
                //     });
                // });

                // Example: Custom assertion for complex role checking
                // o.AddPolicy("CustomRoleCheck", b =>
                // {
                //     b.RequireAssertion(context =>
                //     {
                //         var user = context.User;
                //         // Custom logic to check roles
                //         return user.HasClaim("role", Roles.ADMIN_SUPER) 
                //             || user.HasClaim("role", Roles.STAFF);
                //     });
                // });
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
