

using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.HttpContext;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Identity.Application.Abstractions.Services;

namespace YGZ.Identity.Application.Auths.Commands.ChangePassword;

public class ChangePasswordHandler : ICommandHandler<ChangePasswordCommand, bool>
{
    private readonly IIdentityService _identityService;
    private readonly IUserHttpContext _userHttpContext;
    private readonly ILogger<ChangePasswordHandler> _logger;

    public ChangePasswordHandler(IIdentityService identityService,
                                 IUserHttpContext userHttpContext,
                                 ILogger<ChangePasswordHandler> logger)
    {
        _identityService = identityService;
        _userHttpContext = userHttpContext;
        _logger = logger;
    }

    public async Task<Result<bool>> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var email = _userHttpContext.GetUserEmail();

            var userResult = await _identityService.FindUserAsync(email);

            if (userResult.IsFailure)
            {
                _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                    nameof(_identityService.FindUserAsync), "User not found", new { email });

                return userResult.Error;
            }

            var changePasswordResult = await _identityService.ChangePasswordAsync(userResult.Response!, request.OldPassword, request.NewPassword);

            if (changePasswordResult.IsFailure)
            {
                _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                    nameof(_identityService.ChangePasswordAsync), "Failed to change password", changePasswordResult.Error);

                return changePasswordResult.Error;
            }

            _logger.LogInformation(":::[Handler Information]::: Method: {MethodName}, Information message: {InformationMessage}, Parameters: {@Parameters}",
                nameof(Handle), "Successfully changed password", new { email });

            return true;
        }
        catch (Exception ex)
        {
            var email = _userHttpContext.GetUserEmail();
            var parameters = new { email };
            _logger.LogError(ex, ":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(Handle), ex.Message, parameters);
            
            throw;
        }
    }
}
