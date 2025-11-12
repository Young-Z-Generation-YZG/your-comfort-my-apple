using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Identity.Application.Abstractions.Services;

namespace YGZ.Identity.Application.Auths.Commands.Logout;

public class LogoutHandler : ICommandHandler<LogoutCommand, bool>
{
    private readonly ILogger<LogoutHandler> _logger;
    private readonly IKeycloakService _keycloakService;

    public LogoutHandler(ILogger<LogoutHandler> logger, IKeycloakService keycloakService)
    {
        _logger = logger;
        _keycloakService = keycloakService;
    }

    public async Task<Result<bool>> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // First validate the refresh token with Keycloak
            var validationResult = await _keycloakService.ValidateRefreshTokenAsync(request.RefreshToken);
            
            if (validationResult.IsFailure)
            {
                return validationResult.Error;
            }

            // If validation passes, proceed with logout
            var result = await _keycloakService.LogoutAsync(request.RefreshToken);

            if (result.IsFailure)
            {
                return result.Error;
            }

            _logger.LogInformation("User successfully logged out");

            return result.Response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during logout");
            throw;
        }
    }
}
