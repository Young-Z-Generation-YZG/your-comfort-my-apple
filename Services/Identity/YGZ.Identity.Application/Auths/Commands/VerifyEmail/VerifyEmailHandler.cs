using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Identity.Application.Abstractions.Data;
using YGZ.Identity.Application.Abstractions.Services;
using YGZ.Identity.Domain.Core.Errors;

namespace YGZ.Identity.Application.Auths.Commands.VerifyEmail;

public class VerifyEmailHandler : ICommandHandler<VerifyEmailCommand, bool>
{
    private readonly IIdentityService _identityService;
    private readonly IKeycloakService _keycloakService;
    private readonly ICachedRepository _cachedRepository;
    private readonly ILogger<VerifyEmailHandler> _logger;

    public VerifyEmailHandler(IIdentityService identityService, IKeycloakService keycloakService, ICachedRepository cachedRepository, ILogger<VerifyEmailHandler> logger)
    {
        _identityService = identityService;
        _keycloakService = keycloakService;
        _cachedRepository = cachedRepository;
        _logger = logger;
    }

    public async Task<Result<bool>> Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var otp = await _cachedRepository.GetAsync(request.Email);

            if (otp is null)
            {
                _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                    nameof(_cachedRepository.GetAsync), "OTP expired or not found", new { request.Email });

                return Errors.Auth.ExpiredToken;
            }

            if (otp != request.Otp)
            {
                _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                    nameof(Handle), "Invalid OTP", new { request.Email });

                return Errors.Auth.InvalidOtp;
            }

            var result = await _identityService.VerifyEmailTokenAsync(request.Email, request.Token);

            if (result.IsFailure)
            {
                _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                    nameof(_identityService.VerifyEmailTokenAsync), "Failed to verify email token", result.Error);

                return result.Error;
            }

            var verifyKeycloakUser = await _keycloakService.VerifyEmailAsync(request.Email);

            if (verifyKeycloakUser.IsFailure)
            {
                _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                    nameof(_keycloakService.VerifyEmailAsync), "Failed to verify email in Keycloak", verifyKeycloakUser.Error);

                return verifyKeycloakUser.Error;
            }

            await _cachedRepository.RemoveAsync(request.Email);

            _logger.LogInformation(":::[Handler Information]::: Method: {MethodName}, Information message: {InformationMessage}, Parameters: {@Parameters}",
                nameof(Handle), "Successfully verified email", new { request.Email });

            return result.Response;
        }
        catch (Exception ex)
        {
            var parameters = new { email = request.Email };
            _logger.LogError(ex, ":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(Handle), ex.Message, parameters);
            
            throw;
        }
    }
}
