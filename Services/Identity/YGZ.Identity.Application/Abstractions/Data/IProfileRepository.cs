

using YGZ.Identity.Domain.Users.Entities;

namespace YGZ.Identity.Application.Abstractions.Data;

public interface IProfileRepository
{
    Task<bool> AddAsync(Profile profile);
}
