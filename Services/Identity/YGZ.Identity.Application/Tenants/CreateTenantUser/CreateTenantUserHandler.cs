using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Identity.Application.Abstractions.Services;

namespace YGZ.Identity.Application.Tenants.CreateTenantUser;

public class CreateTenantUserHandler : ICommandHandler<CreateTenantUserCommand, bool>
{
    private readonly IIdentityService _identityService;
    private readonly IKeycloakService _keycloakService;
    private readonly ILogger<CreateTenantUserHandler> _logger;

    public CreateTenantUserHandler(IIdentityService identityService,
                                   IKeycloakService keycloakService,
                                   ILogger<CreateTenantUserHandler> logger)
    {
        _identityService = identityService;
        _keycloakService = keycloakService;
        _logger = logger;
    }

    public async Task<Result<bool>> Handle(CreateTenantUserCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
