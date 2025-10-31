

using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Auth;
using YGZ.Identity.Application.Abstractions.Services;
using YGZ.Identity.Domain.Core.Errors;

namespace YGZ.Identity.Application.Keycloak.Commands;

public class AuthorizationCodeHandler : ICommandHandler<AuthorizationCodeCommand, TokenResponse>
{
    private readonly IKeycloakService _keycloakService;

    public AuthorizationCodeHandler(IKeycloakService keycloakService)
    {
        _keycloakService = keycloakService;
    }

    public async Task<Result<TokenResponse>> Handle(AuthorizationCodeCommand request, CancellationToken cancellationToken)
    {
        var result = await _keycloakService.AuthorizationCode(request);

        if (result is null)
        {
            return Errors.Keycloak.AuthorizationCodeFailed;
        }

        return result;
    }
}
