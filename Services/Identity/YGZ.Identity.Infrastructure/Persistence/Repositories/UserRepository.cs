
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using YGZ.Identity.Application.Abstractions.Data;
using YGZ.Identity.Domain.Users;

namespace YGZ.Identity.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IdentityDbContext _context;
    private readonly DbSet<User> _dbSet;
    private readonly ILogger<UserRepository> _logger;

    public UserRepository(IdentityDbContext context, ILogger<UserRepository> logger)
    {
        _context = context;
        _dbSet = context.Set<User>();
        _logger = logger;
    }

    public DbSet<User> GetDbSet()
    {
        return _dbSet;
    }

    public async Task SaveChange()
    {
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message, nameof(SaveChange));
        }
    }
}
