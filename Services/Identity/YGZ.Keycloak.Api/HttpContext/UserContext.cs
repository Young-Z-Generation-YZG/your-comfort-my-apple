using YGZ.Keycloak.Application.Abstractions;

namespace YGZ.Keycloak.Api.HttpContext;

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
        throw new NotImplementedException();
    }
}
