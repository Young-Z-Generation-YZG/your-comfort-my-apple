

using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Auth;
using YGZ.Identity.Application.Abstractions.Services;

namespace YGZ.Identity.Application.Auths.Commands.RefreshAccessToken;

public class RefreshAccessTokenHandler : ICommandHandler<RefreshAccessTokenCommand, RefreshAccessTokenResponse>
{
    private readonly IKeycloakService _keycloakService;

    public RefreshAccessTokenHandler(IKeycloakService keycloakService)
    {
        _keycloakService = keycloakService;
    }

    public async Task<Result<RefreshAccessTokenResponse>> Handle(RefreshAccessTokenCommand request, CancellationToken cancellationToken)
    {
        var result = await _keycloakService.RefreshAccessTokenAsync(request.RefreshToken);

        if (result.IsFailure)
        {
            return result.Error;
        }

        var response = new RefreshAccessTokenResponse
        {
            AccessToken = result.Response!.AccessToken,
            AccessTokenExpiresInSeconds = result.Response.ExpiresIn,
        };

        return response;
    }
}
