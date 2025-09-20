
using System.Linq.Expressions;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;

namespace YGZ.Discount.Domain.Abstractions.Data;

public interface IGenericRepository<TEntity, TId> where TEntity : class
{
    IQueryable<TEntity> DbSet { get; }
    Task<TEntity> GetByIdAsync(TId id, CancellationToken cancellationToken);
    Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken);
    Task<List<TEntity>> GetAllByFilterAsync(Expression<Func<TEntity, bool>> filterExpression,
                                    CancellationToken cancellationToken,
                                    params Expression<Func<TEntity, object>>[] includes);
    Task<(List<TEntity> orders, int totalRecords, int totalPages)> GetAllAsync(Expression<Func<TEntity, bool>>? filterExpression,
                                                                               int? _page,
                                                                               int? _limit,
                                                                               bool tracked,
                                                                               CancellationToken cancellationToken,
                                                                               params Expression<Func<TEntity, object>>[] includes);
    Task<Result<bool>> AddAsync(TEntity entity, CancellationToken cancellationToken);
    Task<Result<bool>> AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken);
    Task<Result<bool>> UpdateAsync(TEntity entity, CancellationToken cancellationToken);
    Task RemoveAsync(TEntity entity, CancellationToken cancellationToken);
    Task RemoveRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken);
    Task<int> SaveChangesAsync();
}
