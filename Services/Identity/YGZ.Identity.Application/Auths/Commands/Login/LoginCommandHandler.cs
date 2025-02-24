using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Auth;
using YGZ.Identity.Application.Abstractions;

namespace YGZ.Identity.Application.Auths.Commands.Login;

public class LoginCommandHandler : ICommandHandler<LoginCommand, LoginResponse>
{
    private readonly ITokenService _tokenService;
    private readonly IIdentityService _identityService;

    public LoginCommandHandler(ITokenService tokenService, IIdentityService identityService)
    {
        _tokenService = tokenService;
        _identityService = identityService;
    }

    public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _identityService.LoginAsync(request);

        if(existingUser.IsFailure)
        {
            return existingUser.Error;
        }

        var accessToken = await _tokenService.GenerateAccessToken(existingUser.Response!);

        var response = new LoginResponse(accessToken, accessToken, "test");

        return response;
    }
}
