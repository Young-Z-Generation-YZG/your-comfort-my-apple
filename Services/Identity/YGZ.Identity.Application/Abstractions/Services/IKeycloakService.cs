

using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Auth;
using YGZ.Identity.Application.Auths.Commands.Login;
using YGZ.Identity.Application.Auths.Commands.Register;
using YGZ.Identity.Application.Keycloak.Commands;
using YGZ.Identity.Domain.Users.Entities;

namespace YGZ.Identity.Application.Abstractions.Services;

public interface IKeycloakService
{
    Task<Result<KeycloakUser>> GetUserByIdAsync(Guid userId);
    Task<Result<KeycloakUser?>> GetUserByUsernameOrEmailAsync(string usernameOrEmail);
    Task<TokenResponse> GetTokenClientCredentialsTypeAsync();
    Task<TokenResponse> GetKeycloakTokenPairAsync(LoginCommand request);
    Task<TokenResponse> AuthorizationCode(AuthorizationCodeCommand request);
    Task<Result<string>> CreateKeycloakUserAsync(RegisterCommand request);
    Task<Result<bool>> VerifyEmailAsync(string email);
    Task<Result<bool>> SendEmailResetPasswordAsync(string email);
    Task<Result<bool>> ChangePasswordAsync(string email, string currPassword, string newPassword);
    Task<Result<bool>> ResetPasswordAsync(string email, string newPassword);
    Task<Result<TokenResponse>> RefreshAccessTokenAsync(string refreshToken);
}
