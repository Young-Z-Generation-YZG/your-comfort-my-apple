

using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Identity.Application.Auths.Commands.Login;
using YGZ.Identity.Application.Auths.Commands.Register;
using YGZ.Identity.Domain.Users;

namespace YGZ.Identity.Application.Abstractions.Services;

public interface IIdentityService
{
    Task<Result<User>> FindUserAsync(string email);
    Task<Result<bool>> CreateUserAsync(RegisterCommand request);
    Task<Result<User>> LoginAsync(LoginCommand request);
}
