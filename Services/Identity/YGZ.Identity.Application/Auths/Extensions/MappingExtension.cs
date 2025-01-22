

using YGZ.Identity.Application.Auths.Commands.Register;
using YGZ.Identity.Domain.Users;
using YGZ.Identity.Domain.Users.ValueObjects;

namespace YGZ.Identity.Application.Auths.Extensions;

public static class MappingExtension
{
    public static User ToEntity(this RegisterCommand dto)
    {
        return new User()
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            UserName = dto.Email
        };
    }
}
