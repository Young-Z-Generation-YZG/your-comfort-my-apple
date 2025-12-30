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
        try
        {
            var result = await _keycloakService.ImpersonateUserAsync(request.UserId);

            if (result.IsFailure)
            {
                _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                    nameof(_keycloakService.ImpersonateUserAsync), "Failed to impersonate user", new { request.UserId, Error = result.Error });

                return result.Error;
            }

            _logger.LogInformation(":::[Handler Information]::: Method: {MethodName}, Information message: {InformationMessage}, Parameters: {@Parameters}",
                nameof(Handle), "Successfully impersonated user", new { request.UserId });

            return result.Response!;
        }
        catch (Exception ex)
        {
            var parameters = new { userId = request.UserId };
            _logger.LogError(ex, ":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(Handle), ex.Message, parameters);
            
            throw;
        }
    }
}
