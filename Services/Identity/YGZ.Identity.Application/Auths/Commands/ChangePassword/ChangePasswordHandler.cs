

using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Identity.Application.Abstractions.HttpContext;
using YGZ.Identity.Application.Abstractions.Services;

namespace YGZ.Identity.Application.Auths.Commands.ChangePassword;

public class ChangePasswordHandler : ICommandHandler<ChangePasswordCommand, bool>
{
    private readonly IIdentityService _identityService;
    private readonly IUserRequestContext _userContext;

    public ChangePasswordHandler(IIdentityService identityService, IUserRequestContext userContext)
    {
        _identityService = identityService;
        _userContext = userContext;
    }

    public async Task<Result<bool>> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var email = _userContext.GetUserEmail();

        var userAsync = await _identityService.FindUserAsync(email);

        if(userAsync.IsFailure)
        {
            return userAsync.Error;
        }

        var result = await _identityService.ChangePasswordAsync(userAsync.Response!, request.OldPassword, request.NewPassword);

        if (result.IsFailure)
        {
            return result.Error;
        }

        return true;
    }
}
