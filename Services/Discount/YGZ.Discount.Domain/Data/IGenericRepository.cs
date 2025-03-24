
using System.Linq.Expressions;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;

namespace YGZ.Discount.Domain.Data;

public interface IGenericRepository<TEntity, TId> where TEntity : class
{
    Task<TEntity> GetByIdAsync(TId id, CancellationToken cancellationToken);
    Task<(List<TEntity> orders, int totalRecords, int totalPages)> GetAllAsync(Expression<Func<TEntity, bool>>? filterExpression,
                                                                               int? _page,
                                                                               int? _limit,
                                                                               bool tracked,
                                                                               CancellationToken cancellationToken,
                                                                               params Expression<Func<TEntity, object>>[] includes);
    Task<Result<bool>> AddAsync(TEntity entity, CancellationToken cancellationToken);
    Task<Result<bool>> AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken);
    Task RemoveAsync(TEntity entity, CancellationToken cancellationToken);
    Task RemoveRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken);
    Task<int> SaveChangesAsync();
}
