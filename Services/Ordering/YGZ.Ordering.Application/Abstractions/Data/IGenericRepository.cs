

using System.Linq.Expressions;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;

namespace YGZ.Ordering.Application.Abstractions.Data;

public interface IGenericRepository<TEntity, TId> where TEntity : class
{
    Task<TEntity> GetByIdAsync(TId id, CancellationToken cancellationToken);
    Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken, params Expression<Func<TEntity, object>>[] includes);
    Task<(List<TEntity> items, int totalRecords, int totalPages)> GetAllAsync(Expression<Func<TEntity, bool>>? filterExpression,
                                                                               int? page,
                                                                               int? limit,
                                                                               bool tracked,
                                                                               CancellationToken cancellationToken,
                                                                               params Expression<Func<TEntity, object>>[] includes);
    Task<Result<bool>> AddAsync(TEntity entity, CancellationToken cancellationToken);
    Task<bool> AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken);
    Task<bool> UpdateAsync(TEntity entity, CancellationToken cancellationToken);
    Task<bool> RemoveAsync(TEntity entity, CancellationToken cancellationToken);
    Task<bool> RemoveRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken);
    Task SaveChangesAsync();
}
