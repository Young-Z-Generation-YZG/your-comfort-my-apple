

using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Auth;
using YGZ.BuildingBlocks.Shared.Contracts.Keycloak;
using YGZ.Identity.Application.BuilderClasses;
using YGZ.Identity.Application.Keycloak.Commands.AuthorizationCode;
using YGZ.Identity.Domain.Users.Entities;

namespace YGZ.Identity.Application.Abstractions.Services;

public interface IKeycloakService
{
    Task<Result<KeycloakUser>> GetUserByIdAsync(Guid userId);
    Task<Result<KeycloakUser?>> GetUserByUsernameOrEmailAsync(string usernameOrEmail);
    Task<TokenResponse> GetTokenClientCredentialsTypeAsync();
    Task<TokenResponse> GetKeycloakTokenPairAsync(string emailOrUsername, string password);
    Task<TokenResponse> AuthorizationCode(AuthorizationCodeCommand request);
    Task<Result<string>> CreateKeycloakUserAsync(UserRepresentation userRepresentation);
    Task<Result<bool>> VerifyEmailAsync(string email);
    Task<Result<bool>> SendEmailResetPasswordAsync(string email);
    Task<Result<bool>> ChangePasswordAsync(string email, string currPassword, string newPassword);
    Task<Result<bool>> ResetPasswordAsync(string email, string newPassword);
    Task<Result<bool>> ValidateRefreshTokenAsync(string refreshToken);
    Task<Result<TokenResponse>> RefreshAccessTokenAsync(string refreshToken);
    Task<Result<bool>> LogoutAsync(string refreshToken);
    Task<Result<bool>> DeleteKeycloakUserAsync(string keycloakUserId);
    // Task<Result<KeycloakRole>> GetKeycloakRoleByNameHttpAsync(string roleName);
    Task<Result<TokenExchangeResponse>> ImpersonateUserAsync(string userId);
    Task<Result<bool>> AssignRolesToUserAsync(string userId, List<string> roleNames);
}
