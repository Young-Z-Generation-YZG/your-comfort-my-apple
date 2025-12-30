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
                _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                    nameof(_keycloakService.ValidateRefreshTokenAsync), "Invalid refresh token", validationResult.Error);

                return Errors.Keycloak.InvalidRefreshToken;
            }

            var logoutResult = await _keycloakService.LogoutAsync(request.RefreshToken);

            if (logoutResult.IsFailure && logoutResult.Error == Errors.Keycloak.LogoutFailed)
            {
                _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                    nameof(_keycloakService.LogoutAsync), "Failed to logout", logoutResult.Error);

                return Errors.Keycloak.LogoutFailed;
            }

            _logger.LogInformation(":::[Handler Information]::: Method: {MethodName}, Information message: {InformationMessage}, Parameters: {@Parameters}",
                nameof(Handle), "Successfully logged out user", new { hasRefreshToken = !string.IsNullOrEmpty(request.RefreshToken) });

            return true;
        }
        catch (Exception ex)
        {
            var parameters = new { hasRefreshToken = !string.IsNullOrEmpty(request.RefreshToken) };
            _logger.LogError(ex, ":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(Handle), ex.Message, parameters);
            
            throw;
        }
    }
}
