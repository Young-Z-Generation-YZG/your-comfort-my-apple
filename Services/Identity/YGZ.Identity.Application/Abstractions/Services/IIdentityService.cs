

using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Identity.Application.Auths.Commands.Register;
using YGZ.Identity.Domain.Users;

namespace YGZ.Identity.Application.Abstractions.Services;

public interface IIdentityService
{
    Task<Result<User?>> FindUserByEmailAsync(string email);
    Task<Result<User>> FindUserAsync(string email, bool ignoreBaseFilter = false);
    Result<bool> Login(User user, string password);
    Task<Result<User>> CreateUserAsync(RegisterCommand request, Guid userId);
    Task<Result<bool>> AssignRolesAsync(User user, List<string> roleNames);
    Task<Result<bool>> ChangePasswordAsync(User user, string oldPassword, string newPassword);
    Task<Result<string>> GenerateEmailVerificationTokenAsync(User user);
    Task<Result<string>> GenerateResetPasswordTokenAsync(string email);
    Task<Result<bool>> VerifyEmailTokenAsync(string email, string encodedToken);
    Task<Result<bool>> VerifyResetPasswordTokenAsync(string email, string encodedToken, string newPassword);
    Task<Result<List<string>>> GetRolesAsync(User user);
    Task<Result<bool>> DeleteUserAsync(string keycloakUserId);
}
