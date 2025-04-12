

using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Identity.Domain.Users;
using YGZ.Identity.Domain.Users.Entities;
using YGZ.Identity.Domain.Users.ValueObjects;

namespace YGZ.Identity.Application.Abstractions.Data;

public interface IProfileRepository : IGenericRepository<Profile, ProfileId>
{
    Task<Result<Profile>> GetProfileByUser(User user, CancellationToken cancellationToken);
}
