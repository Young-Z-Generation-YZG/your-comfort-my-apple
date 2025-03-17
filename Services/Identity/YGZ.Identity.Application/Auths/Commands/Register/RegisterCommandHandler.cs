using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Identity.Application.Abstractions.Services;

namespace YGZ.Identity.Application.Auths.Commands.Register;

public class RegisterCommandHandler : ICommandHandler<RegisterCommand, bool>
{
    private readonly IIdentityService _identityService;
    private readonly IKeycloakService _keycloakService;
    private readonly ILogger<RegisterCommandHandler> _logger;

    public RegisterCommandHandler(IIdentityService identityService, ILogger<RegisterCommandHandler> logger, IKeycloakService keycloakService)
    {
        _logger = logger;
        _identityService = identityService;
        _keycloakService = keycloakService;
    }

    public async Task<Result<bool>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var result = await _identityService.CreateUserAsync(request);

        if (result.IsFailure)
        {
            return result.Error;
        }

        var keycloakResult = await _keycloakService.CreateKeycloakUserAsync(request);

        if (keycloakResult.IsFailure)
        {
            return keycloakResult.Error;
        }

        return true;
    }
}
