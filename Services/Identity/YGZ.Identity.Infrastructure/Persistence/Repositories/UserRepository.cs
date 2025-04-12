
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
    private readonly IdentityDbContext _context;
    private readonly DbSet<User> _dbSet;
    private readonly ILogger<UserRepository> _logger;

    public UserRepository(IdentityDbContext context, ILogger<UserRepository> logger)
    {
        _context = context;
        _dbSet = context.Set<User>();
        _logger = logger;
    }

    public async Task<Result<User>> GetUserByEmail(string userEmail)
    {
        try
        {
            var result = await _dbSet.FirstOrDefaultAsync(x => x.Email == userEmail);

            if(result is null)
            {
                _logger.LogError("User with email \"{userEmail}\" does not exist", userEmail);

                return Errors.User.DoesNotExist;
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message, nameof(GetUserByEmail));
            throw;
        }
    }

    public async Task<Result<User>> GetUserByEmail(string userEmail, params Expression<Func<User, object>>[] expressions)
    {
        try
        {
            IQueryable<User> query = _dbSet;

            foreach (var expression in expressions)
            {
                query = query.Include(expression);
            }

            var result = await query.FirstOrDefaultAsync(x => x.Email == userEmail);

            if (result is null)
            {
                _logger.LogError("User with email \"{userEmail}\" does not exist", userEmail);

                return Errors.User.DoesNotExist;
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message, nameof(GetUserByEmail));
            throw;
        }
    }

    public async Task<Result<bool>> AddShippingAddressAsync(ShippingAddress shippingAddress, User user)
    {
        try
        {
            user.ShippingAddresses.Add(shippingAddress);

            var result = await _context.SaveChangesAsync();

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

    public DbSet<User> GetDbSet()
    {
        return _dbSet;
    }

    public async Task SaveChange()
    {
        await _context.SaveChangesAsync();
    }
}
