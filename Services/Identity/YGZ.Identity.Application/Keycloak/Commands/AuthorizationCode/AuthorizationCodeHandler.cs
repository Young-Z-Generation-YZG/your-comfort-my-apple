using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Auth;
using YGZ.Identity.Application.Abstractions.Services;
using YGZ.Identity.Domain.Core.Errors;

namespace YGZ.Identity.Application.Keycloak.Commands.AuthorizationCode;

public class AuthorizationCodeHandler : ICommandHandler<AuthorizationCodeCommand, TokenResponse>
{
    private readonly IKeycloakService _keycloakService;
    private readonly ILogger<AuthorizationCodeHandler> _logger;

    public AuthorizationCodeHandler(IKeycloakService keycloakService, ILogger<AuthorizationCodeHandler> logger)
    {
        _keycloakService = keycloakService;
        _logger = logger;
    }

    public async Task<Result<TokenResponse>> Handle(AuthorizationCodeCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _keycloakService.AuthorizationCode(request);

            if (result is null)
            {
                _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                    nameof(_keycloakService.AuthorizationCode), "Authorization code exchange failed", new { hasCode = !string.IsNullOrEmpty(request.Code) });

                return Errors.Keycloak.AuthorizationCodeFailed;
            }

            _logger.LogInformation(":::[Handler Information]::: Method: {MethodName}, Information message: {InformationMessage}, Parameters: {@Parameters}",
                nameof(Handle), "Successfully exchanged authorization code for token", new { hasCode = !string.IsNullOrEmpty(request.Code) });

            return result;
        }
        catch (Exception ex)
        {
            var parameters = new { hasCode = !string.IsNullOrEmpty(request.Code) };
            _logger.LogError(ex, ":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(Handle), ex.Message, parameters);
            
            throw;
        }
    }
}
