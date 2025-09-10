

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

    public async Task<Result<TEntity>> GetByIdAsync(TId id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _dbSet.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (result is null)
            {
                return Errors.Address.NotFound;
            }

            return result;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<Result<List<TEntity>>> GetAllAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _dbSet.ToListAsync(cancellationToken);

            var result = await _dbSet.ToListAsync(cancellationToken);
          
            return result;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    virtual public async Task<Result<bool>> AddAsync(TEntity entity, CancellationToken cancellationToken)
    {
        try
        {
            await _dbSet.AddAsync(entity, cancellationToken);

            var result = await _dbContext.SaveChangesAsync(cancellationToken);

            if (result > 0)
            {
                return true;
            }

            return Errors.Address.CanNotAdd;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    virtual public async Task<Result<bool>> AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    virtual public async Task<Result<bool>> UpdateAsync(TEntity entity, CancellationToken cancellationToken)
    {
        try
        {
            _dbSet.Update(entity);

            var result = await _dbContext.SaveChangesAsync(cancellationToken);

            if (result > 0)
            {
                return true;
            }

            return Errors.Address.CanNotUpdate;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    virtual public async Task<Result<bool>> RemoveAsync(TEntity entity, CancellationToken cancellationToken)
    {
        try
        {
            _dbSet.Remove(entity);

            var result = await _dbContext.SaveChangesAsync(cancellationToken);

            if (result > 0)
            {
                return true;
            }

            return Errors.Address.CanNotDelete;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    virtual public async Task<Result<bool>> RemoveRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}