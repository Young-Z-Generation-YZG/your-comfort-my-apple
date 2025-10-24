using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Identity.Application.Abstractions.Data;
using YGZ.Identity.Domain.Core.Errors;
using YGZ.Identity.Domain.Users;

namespace YGZ.Identity.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IdentityDbContext _dbContext;
    private readonly DbSet<User> _dbSet;
    private readonly ILogger<UserRepository> _logger;

    public UserRepository(IdentityDbContext dbContext, ILogger<UserRepository> logger)
    {
        _dbContext = dbContext;
        _dbSet = dbContext.Set<User>();
        _logger = logger;
    }

    public async Task<Result<User>> GetByIdAsync(string userId, Expression<Func<User, object>>[]? expressions = null, CancellationToken? cancellationToken = null)
    {
        try
        {
            IQueryable<User> query = _dbSet.AsNoTracking();

            if (expressions is not null)
            {
                foreach (var expression in expressions)
                {
                    query = query.Include(expression);
                }
            }

            var result = await query.FirstOrDefaultAsync(x => x.Id == userId, cancellationToken ?? CancellationToken.None);

            if (result is null)
            {
                return Errors.User.DoesNotExist;
            }

            return result;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<Result<User>> GetUserByEmailAsync(string userEmail, Expression<Func<User, object>>[]? expressions = null, CancellationToken? cancellationToken = null)
    {
        try
        {
            IQueryable<User> query = _dbSet.AsNoTracking();

            if (expressions is not null)
            {
                foreach (var expression in expressions)
                {
                    query = query.Include(expression);
                }
            }

            var result = await query.FirstOrDefaultAsync(x => x.Email == userEmail, cancellationToken ?? CancellationToken.None);

            if (result is null)
            {

                return Errors.User.DoesNotExist;
            }

            return result;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<Result<bool>> UpdateUserAsync(User user, CancellationToken? cancellationToken = null)
    {
        try
        {
            var result = _dbSet.Update(user);

            if (result.State == EntityState.Modified)
            {
                var saveChangesResult = await _dbContext.SaveChangesAsync(cancellationToken ?? CancellationToken.None);

                if (saveChangesResult > 0)
                {
                    return true;
                }

                return Errors.User.CannotBeUpdated;
            }
            else
            {
                return Errors.User.CannotBeUpdated;
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    public DbSet<User> GetDbSet()
    {
        return _dbSet;
    }

    public async Task SaveChange()
    {
        await _dbContext.SaveChangesAsync();
    }


}
