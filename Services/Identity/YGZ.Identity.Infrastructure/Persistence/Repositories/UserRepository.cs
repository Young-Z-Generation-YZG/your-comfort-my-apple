
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Identity.Application.Abstractions.Data;
using YGZ.Identity.Domain.Core.Errors;
using YGZ.Identity.Domain.Users;
using YGZ.Identity.Domain.Users.Entities;

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

    public async Task<Result<User>> GetUserByEmailAsync(string userEmail, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _dbSet.FirstOrDefaultAsync(x => x.Email == userEmail, cancellationToken);

            if(result is null)
            {
                _logger.LogError("User with email \"{userEmail}\" does not exist", userEmail);

                return Errors.User.DoesNotExist;
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message, nameof(GetUserByEmailAsync));
            throw;
        }
    }

    public async Task<Result<User>> GetUserByEmailAsync(string userEmail, CancellationToken cancellationToken, params Expression<Func<User, object>>[] expressions)
    {
        try
        {
            IQueryable<User> query = _dbSet;

            foreach (var expression in expressions)
            {
                query = query.Include(expression);
            }

            var result = await query.FirstOrDefaultAsync(x => x.Email == userEmail, cancellationToken);

            if (result is null)
            {
                _logger.LogError("User with email \"{userEmail}\" does not exist", userEmail);

                return Errors.User.DoesNotExist;
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message, nameof(GetUserByEmailAsync));
            throw;
        }
    }

    public async Task<Result<bool>> AddShippingAddressAsync(ShippingAddress shippingAddress, User user, CancellationToken cancellationToken)
    {
        try
        {
            user.ShippingAddresses.Add(shippingAddress);

            var result = await _dbContext.SaveChangesAsync(cancellationToken);

            if (result > 0)
            {
                return true;
            }
            else
            {
                return Errors.Address.CanNotAdd;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message, nameof(AddShippingAddressAsync));

            throw;
        }
    }

    public async Task<Result<bool>> UpdateUserAsync(User user, CancellationToken cancellationToken)
    {
        try
        {
            _dbSet.Update(user);

            var result = await _dbContext.SaveChangesAsync(cancellationToken);

            if (result > 0)
            {
                return true;
            }

            return Errors.User.CannotBeUpdated;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message, $"class:{nameof(UserRepository)} - method:{nameof(UpdateUserAsync)}");

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
