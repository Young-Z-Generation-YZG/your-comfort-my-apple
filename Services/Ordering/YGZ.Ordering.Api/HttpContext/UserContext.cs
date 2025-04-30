using YGZ.Ordering.Application.Abstractions;

namespace YGZ.Ordering.Api.HttpContext;

public class UserContext : IUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<UserContext> _logger;

    public UserContext(IHttpContextAccessor httpContextAccessor, ILogger<UserContext> logger)
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
}
