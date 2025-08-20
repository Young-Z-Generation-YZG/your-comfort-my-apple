using System.Text.Json;
using YGZ.Ordering.Application.Abstractions;

namespace YGZ.Ordering.Api.HttpContext;

public class UserRequestContext : IUserRequestContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<UserRequestContext> _logger;

    public UserRequestContext(IHttpContextAccessor httpContextAccessor, ILogger<UserRequestContext> logger)
    {
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public string GetUserEmail()
    {
        var email = _httpContextAccessor.HttpContext?.User.FindFirst("email")?.Value
                    ?? _httpContextAccessor.HttpContext?.User.Identity?.Name;

        if (string.IsNullOrEmpty(email))
        {
            _logger.LogWarning("No email found in user claims.");
            throw new UnauthorizedAccessException("User email not found in token.");
        }

        return email;
    }

    public string GetUserId()
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirst("sub")?.Value
                    ?? _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            _logger.LogWarning("No user ID found in user claims.");
            throw new UnauthorizedAccessException("User ID not found in token.");
        }

        return userId;
    }

    public List<string> GetUserRoles()
    {
        var user = _httpContextAccessor.HttpContext?.User;

        if (user == null)
        {
            _logger.LogWarning("No user context available.");
            return new List<string>();
        }

        var resourceAccessClaim = user.Claims.FirstOrDefault(c => c.Type == "resource_access")?.Value;
        if (string.IsNullOrEmpty(resourceAccessClaim))
        {
            return new List<string>();
        }

        // Parse the resource_access claim JSON  
        using var jsonDoc = JsonDocument.Parse(resourceAccessClaim);
        var root = jsonDoc.RootElement;

        // Navigate to client-nextjs roles  
        if (root.TryGetProperty("client-nextjs", out var client) &&
            client.TryGetProperty("roles", out var rolesElement))
        {
            return rolesElement.EnumerateArray()
                .Select(role => role.GetString())
                .Where(role => !string.IsNullOrEmpty(role))
                .ToList()!;
        }

        return new List<string>();
    }
}
