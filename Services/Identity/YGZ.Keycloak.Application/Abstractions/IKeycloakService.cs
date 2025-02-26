

using YGZ.BuildingBlocks.Shared.Contracts.Auth;

namespace YGZ.Keycloak.Application.Abstractions;

public interface IKeycloakService
{
    Task<TokenResponse> GetTokenClientCredentialsAsync();
}
