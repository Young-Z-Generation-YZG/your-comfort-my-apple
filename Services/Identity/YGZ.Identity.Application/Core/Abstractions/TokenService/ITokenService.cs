using YGZ.Identity.Domain.Identity.Entities;

namespace YGZ.Identity.Application.Core.Abstractions.TokenService;

public interface ITokenService
{
    string GenerateToken(User user);
}
