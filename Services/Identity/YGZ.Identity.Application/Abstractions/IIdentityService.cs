

using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Identity.Application.Auths.Commands.Login;
using YGZ.Identity.Application.Auths.Commands.Register;
using YGZ.Identity.Domain.Users;

namespace YGZ.Identity.Application.Abstractions;

public interface IIdentityService
{
    Task<Result<User>> FindUserAsync(string email);
    Task<Result<bool>> CreateUserAsync(RegisterCommand request);
    Task<Result<User>> LoginAsync(LoginCommand request);
    Task<Result<string>> GenerateResetPasswordTokenAsync(string email);
    Task<Result<string>> GenerateEmailVerificationTokenAsync(string email);

    List<IdentityRole> GetAllRoles();
    Task<Result<bool>> CreateRoleAsync(string roleName);
    Task<Result<User>> GetAllUsersAsync();
    Task<Result<List<IdentityRole>>> GetUserRolesAsync(string email);
    Task<Result<bool>> RemoveUserFromRoleAsync(string email, string roleName);
    Task<Result<List<Claim>>> GetAllClaims(string email);
    Task<Result<bool>> AddClaimToUserAsync(string email, string claimName,string claimValue);
}
