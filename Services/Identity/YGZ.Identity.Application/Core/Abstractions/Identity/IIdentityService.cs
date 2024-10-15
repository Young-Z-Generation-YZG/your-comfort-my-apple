using YGZ.Identity.Application.Identity.Commands.CreateUser;
using YGZ.Identity.Application.Identity.Common.Dtos;
using YGZ.Identity.Domain.Common.Abstractions;
using YGZ.Identity.Domain.Identity.Entities;

namespace YGZ.Identity.Application.Core.Abstractions.Identity;
public interface IIdentityService
{
    Task<Result<User>> FindUserAsync(FindUserDto request);
    Task<Result<bool>> CreateUserAsync(CreateUserCommand request);
    Task<Result<string>> GenerateEmailVerificationTokenAsync(string email);
    Task<Result<string>> GenerateResetPasswordTokenAsync(string email);
}
