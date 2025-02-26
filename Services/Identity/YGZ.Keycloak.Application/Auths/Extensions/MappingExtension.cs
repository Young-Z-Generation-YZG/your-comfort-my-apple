

using YGZ.Keycloak.Application.Auths.Commands.Register;
using YGZ.Keycloak.Domain.Users;

namespace YGZ.Keycloak.Application.Auths.Extensions;

public static class MappingExtension
{
    public static User ToEntity(this RegisterCommand dto)
    {
        return User.Create(email: dto.Email,
                           passwordHash: dto.Password,
                           firstName: dto.FirstName,
                           lastName: dto.LastName);
    }
}
