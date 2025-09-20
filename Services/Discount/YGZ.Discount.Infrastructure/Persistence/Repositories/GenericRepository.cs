
using System.Linq.Expressions;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Discount.Domain.Abstractions.Data;
using YGZ.Discount.Domain.Core.Primitives;

namespace YGZ.Discount.Infrastructure.Persistence.Repositories;

public class GenericRepository<TEntity, TId> : IGenericRepository<TEntity, TId> where TEntity : Entity<TId> where TId : ValueObject
{
    private readonly DiscountDbContext _discountDbContext;
    protected readonly DbSet<TEntity> _dbSet;
    private readonly ILogger<GenericRepository<TEntity, TId>> _logger;

    public GenericRepository(DiscountDbContext discountDbContext, ILogger<GenericRepository<TEntity, TId>> logger)
    {
        _discountDbContext = discountDbContext;
        _dbSet = discountDbContext.Set<TEntity>();
        _logger = logger;
    }
    public async Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken)
    {
        try
        {
            return await _dbSet.ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error occurred: {@Exception} {@StackTrace}", ex.Message, ex.StackTrace);

            throw;
        }
    }

    public async Task<List<TEntity>> GetAllByFilterAsync(Expression<Func<TEntity, bool>> filterExpression, CancellationToken cancellationToken, params Expression<Func<TEntity, object>>[] includes)
    {
        try
        {
            return await _dbSet.Where(filterExpression).ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error occurred: {@Exception} {@StackTrace}", ex.Message, ex.StackTrace);

            throw;
        }
    }

    virtual public Task<(List<TEntity> orders, int totalRecords, int totalPages)> GetAllAsync(Expression<Func<TEntity, bool>>? filterExpression, int? _page, int? _limit, bool tracked, CancellationToken cancellationToken, params Expression<Func<TEntity, object>>[] includes)
    {
        try
        {
            throw new NotImplementedException();
        }
        catch (Exception ex)
        {
            _logger.LogError("Error occurred: {@Exception} {@StackTrace}", ex.Message, ex.StackTrace);

            throw;
        }
    }

    virtual public async Task<Result<bool>> AddAsync(TEntity entity, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _dbSet.AddAsync(entity, cancellationToken);

            var affectedRows = await SaveChangesAsync();

            return affectedRows > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError("Error occurred: {@Exception} {@StackTrace}", ex.Message, ex.StackTrace);

            return false;
        }
    }

    virtual public async Task<Result<bool>> AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
    {
        try
        {
            await _dbSet.AddRangeAsync(entities, cancellationToken);

            var affectedRows = await SaveChangesAsync();

            return affectedRows > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError("Error occurred: {@Exception} {@StackTrace}", ex.Message, ex.StackTrace);

            return false;
        }
    }


    virtual public async Task<TEntity> GetByIdAsync(TId id, CancellationToken cancellationToken)
    {
        try
        {
            return await _dbSet.FindAsync(new object[] { id }, cancellationToken);
        }
        catch (Exception ex)
        {
            return null!;
        }
    }

    virtual public Task RemoveAsync(TEntity entity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    virtual public Task RemoveRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
    {
        try
        {
            throw new NotImplementedException();
        }
        catch (Exception ex)
        {
            throw new RpcException(new Status(StatusCode.Internal, ex.Message));
        }
    }

    virtual public async Task<int> SaveChangesAsync()
    {
        var affectedRows = await _discountDbContext.SaveChangesAsync();

        return affectedRows;
    }
}
