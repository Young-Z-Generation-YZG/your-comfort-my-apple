using YGZ.Identity.Application.Identity.Commands.CreateUser;
using YGZ.Identity.Domain.Identity.Entities;

namespace YGZ.Identity.Application.Identity.Extensions;

public static class MappingExtension
{
    public static User ToEntity(this CreateUserCommand dto)
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
