

using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Keycloak.Application.Auths.Commands.Login;
using YGZ.Keycloak.Application.Auths.Commands.Register;
using YGZ.Keycloak.Domain.Users;

namespace YGZ.Keycloak.Application.Abstractions;

public interface IIdentityService
{
    Task<Result<User>> FindUserAsync(string email);
    Task<Result<bool>> CreateUserAsync(RegisterCommand request);
    Task<Result<User>> LoginAsync(LoginCommand request);
}
