

using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;

namespace YGZ.Identity.Infrastructure.Extensions;

public class KeycloakClaimsTransformation : IClaimsTransformation
{
    public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        var identity = principal.Identity as ClaimsIdentity;

        var resourceAccess = principal.FindFirst("resource_access")?.Value;

        Console.WriteLine("Claims from Keycloak:");
        foreach (var claim in principal.Claims)
        {
            Console.WriteLine($"{claim.Type}: {claim.Value}");
        }

        if (!string.IsNullOrEmpty(resourceAccess))
        {
            var json = JsonDocument.Parse(resourceAccess);
            if (json.RootElement.TryGetProperty("nextjs-confidential", out var clientRoles)
                && clientRoles.TryGetProperty("roles", out var roles))
            {
                foreach (var role in roles.EnumerateArray())
                {
                    identity.AddClaim(new Claim(ClaimTypes.Role, role.GetString()!));
                }
            }
        }

        return Task.FromResult(principal);
    }
}
