

using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Auth;
using YGZ.Identity.Application.Abstractions.Services;
using YGZ.Identity.Application.Auths.Commands.Login;
using YGZ.Identity.Domain.Core.Errors;

namespace YGZ.Identity.Application.Keycloak.Commands;

public class AuthorizationCodeCommandHandler : ICommandHandler<AuthorizationCodeCommand, TokenResponse>
{
    private readonly ILogger<LoginCommandHandler> _logger;
    private readonly IIdentityService _identityService;
    private readonly IKeycloakService _keycloakService;

    public AuthorizationCodeCommandHandler(ILogger<LoginCommandHandler> logger,
                               IIdentityService identityService,
                               IKeycloakService keycloakService)
    {
        _logger = logger;
        _identityService = identityService;
        _keycloakService = keycloakService;
    }

    public async Task<Result<TokenResponse>> Handle(AuthorizationCodeCommand request, CancellationToken cancellationToken)
    {
        var result = await _keycloakService.AuthorizationCode(request);

        if(result is null)
        {
            return Errors.Keycloak.AuthorizationCodeFailed;
        }

        return result;
    }
}
