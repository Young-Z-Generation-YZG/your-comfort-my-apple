using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Auth;
using YGZ.Keycloak.Application.Abstractions;

namespace YGZ.Keycloak.Application.Auths.Commands.Login;

public class LoginCommandHandler : ICommandHandler<LoginCommand, LoginResponse>
{
    private readonly ILogger<LoginCommandHandler> _logger;
    private readonly IIdentityService _identityService;
    private readonly IKeycloakService _keycloakService;

    public LoginCommandHandler(ILogger<LoginCommandHandler> logger, IIdentityService identityService, IKeycloakService keycloakService)
    {
        _logger = logger;
        _identityService = identityService;
        _keycloakService = keycloakService;
    }

    public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _identityService.LoginAsync(request);

        if (user.IsFailure)
        {
            return user.Error;
        }

        var tokenResponse = await _keycloakService.GetKeycloackUserTokenAsync(request);

        var response = new LoginResponse(tokenResponse, "test", "test");

        return response;
    }
}
