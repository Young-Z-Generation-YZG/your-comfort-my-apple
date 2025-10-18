
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Domain.Core.Primitives;
using YGZ.Ordering.Application.Abstractions.Data;

namespace YGZ.Ordering.Infrastructure.Persistence.Repositories;

public class GenericRepository<TEntity, TId> : IGenericRepository<TEntity, TId> where TEntity : Entity<TId> where TId : ValueObject
{
    private readonly OrderDbContext _orderDbContext;
    protected readonly DbSet<TEntity> _dbSet;
    private readonly ILogger<GenericRepository<TEntity, TId>> _logger;

    public GenericRepository(OrderDbContext orderDbContext, ILogger<GenericRepository<TEntity, TId>> logger)
    {
        _orderDbContext = orderDbContext;
        _dbSet = orderDbContext.Set<TEntity>();
        _logger = logger;
    }

    public async Task<(List<TEntity> items, int totalRecords, int totalPages)> GetAllAsync(Expression<Func<TEntity, bool>>? filterExpression,
                                                                                           int? page,
                                                                                           int? limit,
                                                                                           bool tracked,
                                                                                           CancellationToken cancellationToken,
                                                                                           params Expression<Func<TEntity, object>>[] includes)
    {
        const int defaultPage = 1;
        const int defaultLimit = 10;

        var currentPage = page ?? defaultPage;
        var pageSize = limit ?? defaultLimit;

        var query = _dbSet.AsQueryable();

        // Apply eager loading
        foreach (var include in includes)
        {
            query = query.Include(include);
        }

        // Apply filters
        if (filterExpression is not null)
        {
            query = query.Where(filterExpression);
        }

        // Get total count for pagination
        var totalRecords = await query.CountAsync(cancellationToken);
        var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

        // Apply pagination
        query = query
            .Skip((currentPage - 1) * pageSize)
            .Take(pageSize);

        // Execute query with or without tracking
        var result = tracked
            ? await query.ToListAsync(cancellationToken)
            : await query.AsNoTracking().ToListAsync(cancellationToken);

        return (result, totalRecords, totalPages);
    }

    virtual public async Task<TEntity> GetByIdAsync(TId id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _dbSet.FirstOrDefaultAsync(e => e.Id.Equals(id), cancellationToken);

            return result ?? null!;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting entity by id: {Id}", id);

            throw new Exception(ex.Message, ex);
        }
    }

    virtual public async Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken, params Expression<Func<TEntity, object>>[] includes)
    {
        try
        {
            var query = _dbSet.AsQueryable();

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            var result = await query.FirstOrDefaultAsync(e => e.Id.Equals(id), cancellationToken);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting entity by id with includes: {Id}", id);

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
