
using YGZ.Identity.Application.Core.Abstractions.Identity;
using YGZ.Identity.Application.Core.Abstractions.Messaging;
using YGZ.Identity.Application.Core.Abstractions.TokenService;
using YGZ.Identity.Contracts.Identity.Login;
using YGZ.Identity.Domain.Common.Abstractions;

namespace YGZ.Identity.Application.Identity.Commands.Login;

internal class LoginCommandHandler : ICommandHandler<LoginCommand, LoginResponse>
{
    private readonly IIdentityService _identityService;
    private readonly ITokenService _tokenService;
    public LoginCommandHandler(IIdentityService identityService, ITokenService tokenService)
    {
        _identityService = identityService;
        _tokenService = tokenService;
    }

    public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        Console.WriteLine("Login command");

        var userResult = await _identityService.FindUserAsync(new (request.Email)).ConfigureAwait(false);

        if (userResult.IsFailure)
            return userResult.Error;

        var passwordValidationResult = _identityService.ValidatePasswordAsync(new(userResult.Response!, userResult.Response!.PasswordHash!, request.Password));

        if (passwordValidationResult.IsFailure)
            return passwordValidationResult.Error;

        var token = _tokenService.GenerateToken(userResult.Response!);

        Console.WriteLine("Token generated successfully: " + token);


        return new LoginResponse(userResult.Response!.Email!, userResult.Response.FullName, token, token);
    }
}
