
using YGZ.Identity.Application.Identity.Commands.CreateUser;
using YGZ.Identity.Application.Identity.Common.Abstractions;
using YGZ.Identity.Application.Identity.Common.Dtos;
using YGZ.Identity.Domain.Common.Abstractions;
using YGZ.Identity.Domain.Identity.Entities;

namespace YGZ.Identity.Persistence.Services;

internal class IdentityService : IIdentityService
{
    public Task<Result<bool>> CreateUserAsync(CreateUserCommand request)
    {
        throw new NotImplementedException();
    }

    public Task<Result<User>> FindUserAsync(FindUserDto request)
    {
        throw new NotImplementedException();
    }

    public Task<Result<string>> GenerateEmailVerificationTokenAsync(string email)
    {
        throw new NotImplementedException();
    }

    public Task<Result<string>> GenerateResetPasswordTokenAsync(string email)
    {
        throw new NotImplementedException();
    }
}
