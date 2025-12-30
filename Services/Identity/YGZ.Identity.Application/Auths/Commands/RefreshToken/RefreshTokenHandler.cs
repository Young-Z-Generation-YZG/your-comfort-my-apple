
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Auth;
using YGZ.Identity.Application.Abstractions.Services;

namespace YGZ.Identity.Application.Auths.Commands.RefreshToken;

public class RefreshTokenHandler : ICommandHandler<RefreshTokenCommand, TokenResponse>
{
    private readonly IKeycloakService _keycloakService;
    private readonly ILogger<RefreshTokenHandler> _logger;

    public RefreshTokenHandler(IKeycloakService keycloakService, ILogger<RefreshTokenHandler> logger)
    {
        _keycloakService = keycloakService;
        _logger = logger;
    }

    public async Task<Result<TokenResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // First validate the refresh token with Keycloak
            var validationResult = await _keycloakService.ValidateRefreshTokenAsync(request.RefreshToken);
            
            if (validationResult.IsFailure)
            {
                _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                    nameof(_keycloakService.ValidateRefreshTokenAsync), "Invalid refresh token", validationResult.Error);

                return validationResult.Error;
            }

            // If validation passes, proceed with refresh
            var result = await _keycloakService.RefreshAccessTokenAsync(request.RefreshToken);

            if (result.IsFailure)
            {
                _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                    nameof(_keycloakService.RefreshAccessTokenAsync), "Failed to refresh access token", result.Error);

                return result.Error;
            }

            _logger.LogInformation(":::[Handler Information]::: Method: {MethodName}, Information message: {InformationMessage}, Parameters: {@Parameters}",
                nameof(Handle), "Successfully refreshed access token", new { hasRefreshToken = !string.IsNullOrEmpty(request.RefreshToken) });

            return result.Response!;
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
