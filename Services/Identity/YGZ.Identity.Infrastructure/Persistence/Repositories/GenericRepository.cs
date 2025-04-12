

using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
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

    public GenericRepository(IdentityDbContext discountDbContext, ILogger<GenericRepository<TEntity, TId>> logger)
    {
        _dbContext = discountDbContext;
        _dbSet = discountDbContext.Set<TEntity>();
        _logger = logger;
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
            _logger.LogError(ex, ex.Message, $"class:{nameof(GenericRepository<TEntity, TId>)} - method:{nameof(AddAsync)}");

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
            _logger.LogError(ex, ex.Message, $"class:{nameof(GenericRepository<TEntity, TId>)} - method:{nameof(AddAsync)}");

            throw;
        }
    }

    virtual public async Task<bool> AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    virtual public async Task<bool> UpdateAsync(TEntity entity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    virtual public async Task<bool> RemoveAsync(TEntity entity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    virtual public async Task<bool> RemoveRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}