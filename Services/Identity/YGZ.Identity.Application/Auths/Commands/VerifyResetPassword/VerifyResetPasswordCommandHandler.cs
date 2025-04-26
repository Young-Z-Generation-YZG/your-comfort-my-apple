

using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Identity.Application.Abstractions.Services;
using YGZ.Identity.Domain.Core.Errors;

namespace YGZ.Identity.Application.Auths.Commands.VerifyResetPassword;

public class VerifyResetPasswordCommandHandler : ICommandHandler<VerifyResetPasswordCommand, bool>
{
    private readonly IIdentityService _identityService;
    public VerifyResetPasswordCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<Result<bool>> Handle(VerifyResetPasswordCommand request, CancellationToken cancellationToken)
    {
        if(request.NewPassword != request.ConfirmPassword)
        {
            return Errors.Auth.PasswordNotMatched;
        }

        var result = await _identityService.VerifyResetPasswordTokenAsync(request.Email, request.Token, request.NewPassword);

        if (result.IsFailure)
        {
            return result.Error;
        }

        return result.Response;
    }
}
