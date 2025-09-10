

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Identity.Application.Abstractions.Data;
using YGZ.Identity.Domain.Core.Errors;
using YGZ.Identity.Domain.Users;
using YGZ.Identity.Domain.Users.Entities;
using YGZ.Identity.Domain.Users.ValueObjects;

namespace YGZ.Identity.Infrastructure.Persistence.Repositories;

public class ProfileRepository : GenericRepository<Profile, ProfileId>, IProfileRepository
{
    private readonly IdentityDbContext _dbContext;
    private readonly ILogger<ProfileRepository> _logger;
    public ProfileRepository(IdentityDbContext dbContext, ILogger<ProfileRepository> logger) : base(dbContext, logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result<Profile>> GetProfileByUser(User user, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _dbSet
                .AsNoTracking() // No need to track the entity as we are only reading it => performance optimization
                .FirstOrDefaultAsync(x => x.UserId == user.Id, cancellationToken);

            if(result is null)
            {
                return Errors.Profile.DoesNotExist;
            }

            return result;
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}
