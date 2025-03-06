

using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Auth;
using YGZ.Keycloak.Application.Auths.Commands.Login;
using YGZ.Keycloak.Application.Auths.Commands.Register;

namespace YGZ.Keycloak.Application.Abstractions;

public interface IKeycloakService
{
    Task<TokenResponse> GetTokenClientCredentialsTypeAsync();
    Task<string> GetKeycloackUserTokenAsync(LoginCommand request);
    Task<Result<bool>> CreateKeycloakUserAsync(RegisterCommand request);
}
