

using YGZ.Identity.Domain.Users;

namespace YGZ.Identity.Application.Abstractions;

public interface ITokenService
{
    public Task<string> GenerateAccessToken(User user);
    public Task<string> GenerateRefreshToken();
}
