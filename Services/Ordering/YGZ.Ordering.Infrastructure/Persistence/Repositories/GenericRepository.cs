
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Ordering.Application.Abstractions.Data;
using YGZ.Ordering.Domain.Core.Primitives;

namespace YGZ.Ordering.Infrastructure.Persistence.Repositories;

public class GenericRepository<TEntity, TId> : IGenericRepository<TEntity, TId> where TEntity : Entity<TId> where TId : ValueObject
{
    private readonly OrderDbContext _orderDbContext;
    protected readonly DbSet<TEntity> _dbSet;
    private readonly ILogger<GenericRepository<TEntity, TId>> _logger;

    public GenericRepository(OrderDbContext orderDbContext)
    {
        _orderDbContext = orderDbContext;
        _dbSet = orderDbContext.Set<TEntity>();
    }

    public async Task<(List<TEntity> orders, int totalRecords, int totalPages)> GetAllAsync(Expression<Func<TEntity, bool>>? filterExpression,
                                                                                            int? _page,
                                                                                            int? _limit,
                                                                                            bool tracked,
                                                                                            CancellationToken cancellationToken,
                                                                                            params Expression<Func<TEntity, object>>[] includes)
    {
        var defaultPage = 1;
        var defaultLimit = 10;

        try
        {
            var query = _dbSet.AsQueryable();

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            if (filterExpression is not null)
            {
                query = query.Where(filterExpression);
            }

            var totalRecords = await query.CountAsync(cancellationToken);
            var totalPages = (int)Math.Ceiling((double)totalRecords / _limit ?? defaultLimit);

            if (_page.HasValue && _limit.HasValue)
            {
                query = query.Skip((_page.Value - 1) * _limit.Value).Take(_limit.Value);
            } else
            {
                query = query.Skip((defaultPage - 1) * defaultLimit).Take(defaultLimit);
            }

            var result = tracked ? await query.ToListAsync(cancellationToken) : await query.AsNoTracking().ToListAsync(cancellationToken);

            return (result ?? new List<TEntity>(), totalRecords, totalPages);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    virtual public async Task<TEntity> GetByIdAsync(TId id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _dbSet.FirstOrDefaultAsync(e => e.Id.Equals(id));

            return result ?? null!;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting entity by id: {Id}", id);

            throw new Exception(ex.Message, ex);
        }
    }

    virtual public async Task<Result<bool>> AddAsync(TEntity entity, CancellationToken cancellationToken)
    {
        try
        {
            await _dbSet.AddAsync(entity);
            await SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    virtual public async Task<bool> AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
    {
        try
        {
            await _dbSet.AddRangeAsync(entities);
            await SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding entities: {Entities}", entities);

            return false;
        }
    }

    virtual public Task<bool> RemoveAsync(TEntity entity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    virtual public Task<bool> RemoveRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    virtual public async Task<bool> UpdateAsync(TEntity entity, CancellationToken cancellationToken)
    {
        try
        {
            _dbSet.Update(entity);
            await SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating entity: {Entity}", entity);

            return false;
        }
    }

    virtual public async Task SaveChangesAsync()
    {
        await _orderDbContext.SaveChangesAsync();
    }
}
