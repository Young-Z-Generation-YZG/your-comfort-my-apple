
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Domain.Core.Primitives;
using YGZ.Ordering.Application.Abstractions.Data;
using YGZ.Ordering.Domain.Core.Errors;

namespace YGZ.Ordering.Infrastructure.Persistence.Repositories;

public class GenericRepository<TEntity, TId> : IGenericRepository<TEntity, TId> where TEntity : Entity<TId> where TId : ValueObject
{
    private readonly OrderDbContext _dbContext;
    protected readonly DbSet<TEntity> _dbSet;
    private readonly ILogger<GenericRepository<TEntity, TId>> _logger;

    public GenericRepository(OrderDbContext dbContext, ILogger<GenericRepository<TEntity, TId>> logger)
    {
        _dbContext = dbContext;
        _dbSet = dbContext.Set<TEntity>();
        _logger = logger;
    }

    virtual public async Task<(List<TEntity> items, int totalRecords, int totalPages)> GetAllAsync(Expression<Func<TEntity, bool>>? filterExpression = null,
                                                                                           Expression<Func<TEntity, object>>[]? includeExpressions = null,
                                                                                           int? page = null,
                                                                                           int? limit = null,
                                                                                           CancellationToken? cancellationToken = null)
    {

        var currentPage = page ?? 1;
        var pageSize = limit ?? 10;

        var query = _dbSet.AsNoTracking();

        if (includeExpressions is not null)
        {
            foreach (var include in includeExpressions)
            {
                query = query.Include(include);
            }
        }

        if (filterExpression is not null)
        {
            query = query.Where(filterExpression);
        }

        var totalRecords = await query.CountAsync(cancellationToken ?? CancellationToken.None);
        var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

        query = query
            .Skip((currentPage - 1) * pageSize)
            .Take(pageSize);

        var result = await query.ToListAsync(cancellationToken ?? CancellationToken.None);

        return (result, totalRecords, totalPages);
    }

    virtual public async Task<TEntity?> GetByIdAsync(TId id,
                                                     Expression<Func<TEntity, object>>[]? expressions = null,
                                                     CancellationToken? cancellationToken = null)
    {
        var query = _dbSet.AsNoTracking();

        if (expressions is not null)
        {
            foreach (var expression in expressions)
            {
                query = query.Include(expression);
            }
        }

        try
        {
            var result = await query.FirstOrDefaultAsync(x => x.Id == id, cancellationToken ?? CancellationToken.None);

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

            return Errors.Common.AddFailure;
        }
        catch (Exception)
        {
            throw;
        }
    }

    virtual public async Task<Result<bool>> UpdateAsync(TEntity entity, CancellationToken? cancellationToken)
    {
        try
        {
            _dbSet.Update(entity);

            var result = await _dbContext.SaveChangesAsync(cancellationToken ?? CancellationToken.None);

            if (result > 0)
            {
                return true;
            }

            return Errors.Common.UpdateFailure;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? filterExpression = null,
                                                 Expression<Func<TEntity, object>>[]? includeExpressions = null,
                                                 CancellationToken? cancellationToken = null)
    {
        try
        {
            var query = _dbSet.AsNoTracking();

            if (includeExpressions is not null)
            {
                foreach (var include in includeExpressions)
                {
                    query = query.Include(include);
                }
            }

            if (filterExpression is not null)
            {
                query = query.Where(filterExpression);
            }

            return await query.ToListAsync(cancellationToken ?? CancellationToken.None);
        }
        catch (Exception)
        {
            throw;
        }
    }
}
