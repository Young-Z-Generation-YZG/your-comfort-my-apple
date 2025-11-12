

using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Auth;
using YGZ.Identity.Application.Abstractions.Services;

namespace YGZ.Identity.Application.Auths.Commands.RefreshToken;

public class RefreshTokenHandler : ICommandHandler<RefreshTokenCommand, TokenResponse>
{
    private readonly IKeycloakService _keycloakService;

    public RefreshTokenHandler(IKeycloakService keycloakService)
    {
        _keycloakService = keycloakService;
    }

    public async Task<Result<TokenResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        // First validate the refresh token with Keycloak
        var validationResult = await _keycloakService.ValidateRefreshTokenAsync(request.RefreshToken);
        
        if (validationResult.IsFailure)
        {
            return validationResult.Error;
        }

        // If validation passes, proceed with refresh
        var result = await _keycloakService.RefreshAccessTokenAsync(request.RefreshToken);

        if (result.IsFailure)
        {
            return result.Error;
        }

        return result.Response!;
    }
}
