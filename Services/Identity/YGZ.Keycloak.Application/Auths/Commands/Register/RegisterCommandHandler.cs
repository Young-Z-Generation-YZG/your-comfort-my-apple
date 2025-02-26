

using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Keycloak.Application.Abstractions;

namespace YGZ.Keycloak.Application.Auths.Commands.Register;

public class RegisterCommandHandler : ICommandHandler<RegisterCommand, bool>
{
    private readonly IIdentityService _identityService;
    private readonly ILogger<RegisterCommandHandler> _logger;

    public RegisterCommandHandler(IIdentityService identityService, ILogger<RegisterCommandHandler> logger)
    {
        _identityService = identityService;
        _logger = logger;
    }

    public async Task<Result<bool>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var result = await _identityService.CreateUserAsync(request);

        if(result.IsFailure)
        {
            return result.Error;
        }

        return true;
    }
}
