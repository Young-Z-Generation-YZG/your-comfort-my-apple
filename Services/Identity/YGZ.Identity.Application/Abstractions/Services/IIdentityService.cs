

using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Identity.Application.Auths.Commands.Login;
using YGZ.Identity.Application.Auths.Commands.Register;
using YGZ.Identity.Domain.Users;

namespace YGZ.Identity.Application.Abstractions.Services;

public interface IIdentityService
{
    Task<Result<User?>> FindUserByEmailAsync(string email);
    Task<Result<User>> FindUserAsync(string email);
    Task<Result<User>> FindUserAsyncIgnoreFilters(string email);
    Task<Result<User>> LoginAsync(LoginCommand request);
    Task<Result<bool>> CreateUserAsync(RegisterCommand request, Guid userId);
    Task<Result<bool>> ChangePasswordAsync(User user, string oldPassword, string newPassword);
    Task<Result<string>> GenerateEmailVerificationTokenAsync(string email);
    Task<Result<string>> GenerateResetPasswordTokenAsync(string email);
    Task<Result<bool>> VerifyEmailTokenAsync(string email, string encodedToken);
    Task<Result<bool>> VerifyResetPasswordTokenAsync(string email, string encodedToken, string newPassword);
    Task<Result<List<string>>> GetRolesAsync(User user);
    Task<Result<bool>> DeleteUserAsync(string keycloakUserId);
}
