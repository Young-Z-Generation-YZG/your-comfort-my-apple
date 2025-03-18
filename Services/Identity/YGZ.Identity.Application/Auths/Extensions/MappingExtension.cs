using YGZ.Identity.Application.Auths.Commands.Register;
using YGZ.Identity.Domain.Users;

namespace YGZ.Identity.Application.Auths.Extensions;

public static class MappingExtension
{
    public static User ToEntity(this RegisterCommand dto, Guid userId)
    {
        return User.Create(guid: userId,
                           email: dto.Email,
                           passwordHash: dto.Password,
                           firstName: dto.FirstName,
                           lastName: dto.LastName);
    }
}
