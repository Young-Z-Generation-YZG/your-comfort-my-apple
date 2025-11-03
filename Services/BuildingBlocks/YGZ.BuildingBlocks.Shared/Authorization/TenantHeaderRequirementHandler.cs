using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace YGZ.BuildingBlocks.Shared.Authorization;

/// <summary>
/// Authorization handler that requires X-Tenant-Id header for specific roles.
/// Roles are provided by the requirement instance per-policy.
/// </summary>
public class TenantHeaderRequirementHandler : AuthorizationHandler<TenantHeaderRequirement>
{
    private const string TenantIdHeaderName = "X-TenantId";
    private const string KeycloakClientId = "ybstore-admin-dashboard-client-id";
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TenantHeaderRequirementHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        TenantHeaderRequirement requirement)
    {
        // Extract roles from Keycloak resource_access claim
        var userRoles = GetUserRoles(context.User);

        // If current user has any role that requires tenant header, enforce it
        if (requirement.RolesRequiringTenantHeader.Overlaps(userRoles))
        {
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext == null)
            {
                // Can't verify header without HttpContext, fail for safety
                context.Fail();
                return Task.CompletedTask;
            }

            var tenantId = httpContext.Request.Headers[TenantIdHeaderName].FirstOrDefault();

            if (string.IsNullOrWhiteSpace(tenantId))
            {
                context.Fail();
                return Task.CompletedTask;
            }
        }

        context.Succeed(requirement);

        return Task.CompletedTask;
    }

    /// <summary>
    /// Extracts user roles from Keycloak resource_access claim.
    /// </summary>
    private static List<string> GetUserRoles(System.Security.Claims.ClaimsPrincipal user)
    {
        var roles = new List<string>();

        var resourceAccessClaim = user.FindFirst("resource_access")?.Value;
        if (string.IsNullOrEmpty(resourceAccessClaim))
        {
            return roles;
        }

        try
        {
            using var jsonDoc = JsonDocument.Parse(resourceAccessClaim);
            var root = jsonDoc.RootElement;

            // Get client roles from resource_access
            if (root.TryGetProperty(KeycloakClientId, out var client) &&
                client.TryGetProperty("roles", out var rolesElement))
            {
                roles = rolesElement.EnumerateArray()
                    .Select(r => r.GetString())
                    .Where(r => !string.IsNullOrEmpty(r))
                    .ToList()!;
            }
        }
        catch (JsonException)
        {
            // If JSON parsing fails, return empty list
        }

        return roles;
    }
}

