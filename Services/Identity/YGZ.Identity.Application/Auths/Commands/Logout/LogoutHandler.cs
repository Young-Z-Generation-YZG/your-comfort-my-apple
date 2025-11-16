using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Identity.Application.Abstractions.Services;
using YGZ.Identity.Domain.Core.Errors;

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
            var validationResult = await _keycloakService.ValidateRefreshTokenAsync(request.RefreshToken);
            
            if (validationResult.IsFailure && validationResult.Error == Errors.Keycloak.InvalidRefreshToken)
            {
                return Errors.Keycloak.InvalidRefreshToken;
            }

            var logoutResult = await _keycloakService.LogoutAsync(request.RefreshToken);

            if (logoutResult.IsFailure && logoutResult.Error == Errors.Keycloak.LogoutFailed)
            {
                return Errors.Keycloak.LogoutFailed;
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception occurred while logging out user: {ErrorMessage}", ex.Message);
            throw;
        }
    }
}
