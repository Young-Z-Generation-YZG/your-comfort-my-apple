

using YGZ.Keycloak.Application.Auths.Commands.Register;
using YGZ.Keycloak.Domain.Users;

namespace YGZ.Keycloak.Application.Auths.Extensions;

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
