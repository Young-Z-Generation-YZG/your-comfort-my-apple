

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using YGZ.Identity.Application.Abstractions.Data;
using YGZ.Identity.Domain.Users.Entities;

namespace YGZ.Identity.Infrastructure.Persistence.Repositories;

public class ProfileRepository : IProfileRepository
{
    private readonly IdentityDbContext _context;
    private readonly DbSet<Profile> _dbSet;
    private readonly ILogger<ProfileRepository> _logger;
    public ProfileRepository(IdentityDbContext context, ILogger<ProfileRepository> logger)
    {
        _context = context;
        _dbSet = context.Set<Profile>();
        _logger = logger;
    }
    public async Task<bool> AddAsync(Profile profile)
    {
        try
        {
            await _context.Profiles.AddAsync(profile);

            await _context.SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message, nameof(AddAsync));
            return false;
        }
    }
}
