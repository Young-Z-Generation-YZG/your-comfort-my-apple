using System.Linq;
using System.Linq.Expressions;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;

namespace YGZ.Ordering.Application.Abstractions.Data;

public interface IGenericRepository<TEntity, TId> where TEntity : class
{
    Task<TEntity?> GetByIdAsync(TId id,
                                Expression<Func<TEntity, object>>[]? includes = null,
                                CancellationToken? cancellationToken = null);

    Task<(List<TEntity> items, int totalRecords, int totalPages)> GetAllAsync(Expression<Func<TEntity, bool>>? filterExpression = null,
                                                                              Expression<Func<TEntity, object>>[]? includeExpressions = null,
                                                                              Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
                                                                              int? page = null,
                                                                              int? limit = null,
                                                                              CancellationToken? cancellationToken = null);

    Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? filterExpression = null,
                                    Expression<Func<TEntity, object>>[]? includeExpressions = null,
                                    Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
                                    CancellationToken? cancellationToken = null);
    Task<Result<bool>> AddAsync(TEntity entity, CancellationToken? cancellationToken);
    Task<Result<bool>> UpdateAsync(TEntity entity, CancellationToken? cancellationToken);
    Task<Result<bool>> UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken? cancellationToken);
}
