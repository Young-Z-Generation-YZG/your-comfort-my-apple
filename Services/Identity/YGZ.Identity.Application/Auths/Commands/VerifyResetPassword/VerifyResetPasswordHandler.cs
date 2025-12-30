
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Identity.Application.Abstractions.Services;
using YGZ.Identity.Domain.Core.Errors;

namespace YGZ.Identity.Application.Auths.Commands.VerifyResetPassword;

public class VerifyResetPasswordHandler : ICommandHandler<VerifyResetPasswordCommand, bool>
{
    private readonly IIdentityService _identityService;
    private readonly ILogger<VerifyResetPasswordHandler> _logger;

    public VerifyResetPasswordHandler(IIdentityService identityService, ILogger<VerifyResetPasswordHandler> logger)
    {
        _identityService = identityService;
        _logger = logger;
    }

    public async Task<Result<bool>> Handle(VerifyResetPasswordCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.NewPassword != request.ConfirmPassword)
            {
                _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                    nameof(Handle), "Password and confirm password do not match", new { request.Email });

                return Errors.Auth.ResetPasswordNotMatched;
            }

            var result = await _identityService.VerifyResetPasswordTokenAsync(request.Email, request.Token, request.NewPassword);

            if (result.IsFailure)
            {
                _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                    nameof(_identityService.VerifyResetPasswordTokenAsync), "Failed to verify reset password token", result.Error);

                return result.Error;
            }

            _logger.LogInformation(":::[Handler Information]::: Method: {MethodName}, Information message: {InformationMessage}, Parameters: {@Parameters}",
                nameof(Handle), "Successfully verified and reset password", new { request.Email });

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
