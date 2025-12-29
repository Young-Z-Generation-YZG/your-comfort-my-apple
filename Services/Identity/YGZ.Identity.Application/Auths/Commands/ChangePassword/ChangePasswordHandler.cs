

using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.HttpContext;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Identity.Application.Abstractions.Services;
using YGZ.Identity.Application.Users.Queries.GetProfile;
using YGZ.Identity.Domain.Core.Errors;

namespace YGZ.Identity.Application.Auths.Commands.ChangePassword;

public class ChangePasswordHandler : ICommandHandler<ChangePasswordCommand, bool>
{
    private readonly IIdentityService _identityService;
    private readonly IUserHttpContext _userHttpContext;
    private readonly ILogger<GetMeHandler> _logger;


    public ChangePasswordHandler(IIdentityService identityService,
                                 IUserHttpContext userHttpContext,
                                 ILogger<GetMeHandler> logger)
    {
        _identityService = identityService;
        _userHttpContext = userHttpContext;
        _logger = logger;
    }

    public async Task<Result<bool>> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var email = _userHttpContext.GetUserEmail();

        var userResult = await _identityService.FindUserAsync(email);

        if (userResult.IsFailure)
        {
            return userResult.Error;
        }

        var changePasswordResult = await _identityService.ChangePasswordAsync(userResult.Response!, request.OldPassword, request.NewPassword);

        if (changePasswordResult.IsFailure)
        {
            return changePasswordResult.Error;
        }

        return true;
    }
}
