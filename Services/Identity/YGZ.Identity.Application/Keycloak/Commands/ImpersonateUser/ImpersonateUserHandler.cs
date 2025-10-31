using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Keycloak;
using YGZ.Identity.Application.Abstractions.Services;

namespace YGZ.Identity.Application.Keycloak.Commands.ImpersonateUser;

public class ImpersonateUserHandler : ICommandHandler<ImpersonateUserCommand, TokenExchangeResponse>
{
    private readonly ILogger<ImpersonateUserHandler> _logger;
    private readonly IKeycloakService _keycloakService;

    public ImpersonateUserHandler(ILogger<ImpersonateUserHandler> logger,
                                  IKeycloakService keycloakService)
    {
        _logger = logger;
        _keycloakService = keycloakService;
    }

    public async Task<Result<TokenExchangeResponse>> Handle(ImpersonateUserCommand request, CancellationToken cancellationToken)
    {
        var result = await _keycloakService.ImpersonateUserAsync(request.UserId);

        if (result.IsFailure)
        {
            return result.Error;
        }

        return result.Response!;
    }
}
