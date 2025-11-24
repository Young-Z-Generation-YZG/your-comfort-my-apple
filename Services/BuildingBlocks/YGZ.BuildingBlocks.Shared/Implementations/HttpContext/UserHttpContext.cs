using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.HttpContext;

namespace YGZ.BuildingBlocks.Shared.Implementations.HttpContext;

public class UserHttpContext : IUserHttpContext
{
    private readonly ILogger<UserHttpContext> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserHttpContext(ILogger<UserHttpContext> logger, IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public string GetUserEmail()
    {
        var email = _httpContextAccessor.HttpContext?.User.FindFirst("email")?.Value ?? _httpContextAccessor.HttpContext?.User.Identity?.Name;

        if (string.IsNullOrEmpty(email))
        {
            _logger.LogWarning("No email found in user claims.");
            throw new UnauthorizedAccessException("User email not found in token.");
        }

        return email;
    }

    public string GetUserId()
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
            _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            _logger.LogWarning("No user ID found in user claims.");
            throw new UnauthorizedAccessException("User ID not found in token.");
        }

        return userId;
    }

    public List<string> GetUserRoles()
    {
        var roles = new List<string>();

        var user = _httpContextAccessor.HttpContext?.User;
        if (user == null)
        {
            _logger.LogWarning("No user context available.");
            return roles;
        }

        var resourceAccessClaim = user.FindFirst("resource_access")?.Value;
        if (string.IsNullOrEmpty(resourceAccessClaim))
        {
            _logger.LogDebug("No resource_access claim found in user token.");
            return roles;
        }

        try
        {
            using var jsonDoc = JsonDocument.Parse(resourceAccessClaim);
            var root = jsonDoc.RootElement;

            const string keycloakClientId = "ybstore-admin-dashboard-client-id";

            // Get client roles from resource_access
            if (root.TryGetProperty(keycloakClientId, out var client) &&
                client.TryGetProperty("roles", out var rolesElement))
            {
                roles = rolesElement.EnumerateArray()
                    .Select(r => r.GetString())
                    .Where(r => !string.IsNullOrEmpty(r))
                    .ToList()!;
            }
            else
            {
                _logger.LogDebug("No roles found for client '{ClientId}' in resource_access claim.", keycloakClientId);
            }
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Failed to parse resource_access claim JSON.");
        }

        return roles;
    }
}
