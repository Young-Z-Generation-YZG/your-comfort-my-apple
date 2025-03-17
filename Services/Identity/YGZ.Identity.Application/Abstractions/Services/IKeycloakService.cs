

using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Auth;
using YGZ.Identity.Application.Auths.Commands.Login;
using YGZ.Identity.Application.Auths.Commands.Register;

namespace YGZ.Identity.Application.Abstractions.Services;

public interface IKeycloakService
{
    Task<TokenResponse> GetTokenClientCredentialsTypeAsync();
    Task<string> GetKeycloackUserTokenAsync(LoginCommand request);
    Task<Result<bool>> CreateKeycloakUserAsync(RegisterCommand request);
}
