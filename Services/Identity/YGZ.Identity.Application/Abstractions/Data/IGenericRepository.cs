
using System.Linq.Expressions;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;

namespace YGZ.Identity.Application.Abstractions.Data;

public interface IGenericRepository<TEntity, TId> where TEntity : class
{
    Task<Result<List<TEntity>>> GetAllAsync(CancellationToken cancellationToken);
    Task<Result<bool>> AddAsync(TEntity entity, CancellationToken cancellationToken);
    Task<bool> AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken);
    Task<bool> UpdateAsync(TEntity entity, CancellationToken cancellationToken);
    Task<bool> RemoveAsync(TEntity entity, CancellationToken cancellationToken);
    Task<bool> RemoveRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken);
}
