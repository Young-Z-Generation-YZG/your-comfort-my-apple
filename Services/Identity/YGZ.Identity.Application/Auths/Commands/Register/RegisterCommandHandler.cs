using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Identity.Application.Abstractions.Services;
using YGZ.Identity.Domain.Core.Errors;

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
        var keycloakUser = await _keycloakService.GetUserByUsernameOrEmailAsync(request.Email);

        if(keycloakUser.Response is not null)
        {
            return Errors.Keycloak.UserAlreadyExist;
        }

        var keycloakResult = await _keycloakService.CreateKeycloakUserAsync(request);

        if(keycloakResult.IsFailure)
        {
            return keycloakResult.Error;
        }

        var result = await _identityService.CreateUserAsync(request, new Guid(keycloakResult.Response!));

        if (result.IsFailure)
        {
            return result.Error;
        }

        if (keycloakResult.IsFailure)
        {
            return keycloakResult.Error;
        }

        return true;
    }
}
