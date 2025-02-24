 using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Auth;
using YGZ.BuildingBlocks.Shared.Errors;

namespace YGZ.Keycloak.Application.Auths.Commands.Login;

public class LoginCommandHandler : ICommandHandler<LoginCommand, LoginResponse>
{
    private readonly ILogger<LoginCommandHandler> _logger;

    public LoginCommandHandler(ILogger<LoginCommandHandler> logger)
    {
        _logger = logger;
    }

    public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        _logger.LogInformation("Login request - Email: {Email}, Password: {Password}",
            request.Email, request.Password);

        var reponse = new LoginResponse("Access token", "refresh token", "10p");

        return reponse;
    }
}
