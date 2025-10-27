

using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Identity.Application.Abstractions.Data;
using YGZ.Identity.Domain.Core.Errors;
using YGZ.Identity.Domain.Core.Primitives;

namespace YGZ.Identity.Infrastructure.Persistence.Repositories;

public class GenericRepository<TEntity, TId> : IGenericRepository<TEntity, TId> where TEntity : Entity<TId> where TId : ValueObject
{
    private readonly IdentityDbContext _dbContext;
    protected readonly DbSet<TEntity> _dbSet;
    private readonly ILogger<GenericRepository<TEntity, TId>> _logger;

    public GenericRepository(IdentityDbContext dbContext, ILogger<GenericRepository<TEntity, TId>> logger)
    {
        _dbContext = dbContext;
        _dbSet = dbContext.Set<TEntity>();
        _logger = logger;
    }

    public async Task<Result<TEntity>> GetByIdAsync(TId id, Expression<Func<TEntity, object>>[] expressions = null!, CancellationToken? cancellationToken = null)
    {
        try
        {
            var query = _dbSet.AsNoTracking();

            if (expressions is not null)
            {
                foreach (var expression in expressions)
                {
                    query = query.Include(expression);
                }
            }

            var result = await query.FirstOrDefaultAsync(x => x.Id == id, cancellationToken ?? CancellationToken.None);

            if (result is null)
            {
                return Errors.Address.NotFound;
            }

            return result;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<Result<List<TEntity>>> GetAllAsync(CancellationToken? cancellationToken = null)
    {
        try
        {
            var result = await _dbSet.ToListAsync(cancellationToken ?? CancellationToken.None);

            return result;
        }
        catch (Exception)
        {
            throw;
        }
    }

    virtual public async Task<Result<List<TEntity>>> GetAllByFilterAsync(Expression<Func<TEntity, bool>>? filterExpression = null, CancellationToken? cancellationToken = null)
    {
        try
        {
            var query = _dbSet.AsNoTracking();

            if (filterExpression is not null)
            {
                query = query.Where(filterExpression);
            }

            var result = await query.ToListAsync(cancellationToken ?? CancellationToken.None);

            return result;
        }
        catch (Exception)
        {
            throw;
        }
    }

    virtual public async Task<Result<bool>> AddAsync(TEntity entity, CancellationToken? cancellationToken = null)
    {
        try
        {
            await _dbSet.AddAsync(entity, cancellationToken ?? CancellationToken.None);

            var result = await _dbContext.SaveChangesAsync(cancellationToken ?? CancellationToken.None);

            if (result > 0)
            {
                return true;
            }

            return Errors.Address.CanNotAdd;
        }
        catch (Exception)
        {
            throw;
        }
    }

    virtual public async Task<Result<bool>> AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken? cancellationToken = null)
    {
        try
        {
            await _dbSet.AddRangeAsync(entities, cancellationToken ?? CancellationToken.None);

            var result = await _dbContext.SaveChangesAsync(cancellationToken ?? CancellationToken.None);

            if (result > 0)
            {
                return true;
            }

            return Errors.Address.CanNotAdd;
        }
        catch (Exception)
        {
            throw;
        }
    }

    virtual public async Task<Result<bool>> UpdateAsync(TEntity entity, CancellationToken? cancellationToken = null)
    {
        try
        {
            _dbSet.Update(entity);

            var result = await _dbContext.SaveChangesAsync(cancellationToken ?? CancellationToken.None);

            if (result > 0)
            {
                return true;
            }

            return Errors.Address.CanNotUpdate;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<Result<bool>> UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken? cancellationToken = null)
    {
        try
        {
            _dbSet.UpdateRange(entities);

            var result = await _dbContext.SaveChangesAsync(cancellationToken ?? CancellationToken.None);

            return result > 0 ? true : Errors.Address.CanNotUpdate;
        }
        catch (Exception)
        {
            throw;
        }
    }


    virtual public async Task<Result<bool>> RemoveAsync(TEntity entity, CancellationToken? cancellationToken = null)
    {
        try
        {
            _dbSet.Remove(entity);

            var result = await _dbContext.SaveChangesAsync(cancellationToken ?? CancellationToken.None);

            if (result > 0)
            {
                return true;
            }

            return Errors.Address.CanNotDelete;
        }
        catch (Exception)
        {
            throw;
        }
    }

    virtual public async Task<Result<bool>> RemoveRangeAsync(IEnumerable<TEntity> entities, CancellationToken? cancellationToken = null)
    {
        try
        {
            _dbSet.RemoveRange(entities);

            var result = await _dbContext.SaveChangesAsync(cancellationToken ?? CancellationToken.None);

            if (result > 0)
            {
                return true;
            }

            return Errors.Address.CanNotDelete;
        }
        catch (Exception)
        {
            throw;
        }
    }
}