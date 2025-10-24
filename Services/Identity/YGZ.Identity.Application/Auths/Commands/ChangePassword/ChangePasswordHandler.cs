

using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.HttpContext;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Identity.Application.Abstractions.Services;
using YGZ.Identity.Application.Users.Queries.GetProfile;

namespace YGZ.Identity.Application.Auths.Commands.ChangePassword;

public class ChangePasswordHandler : ICommandHandler<ChangePasswordCommand, bool>
{
    private readonly IIdentityService _identityService;
    private readonly IUserHttpContext _userHttpContext;
    private readonly ILogger<GetMeQueryHandler> _logger;


    public ChangePasswordHandler(IIdentityService identityService,
                                 IUserHttpContext userHttpContext,
                                 ILogger<GetMeQueryHandler> logger)
    {
        _identityService = identityService;
        _userHttpContext = userHttpContext;
        _logger = logger;
    }

    public async Task<Result<bool>> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var email = _userHttpContext.GetUserEmail();

        var userAsync = await _identityService.FindUserAsync(email);

        if (userAsync.IsFailure)
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
