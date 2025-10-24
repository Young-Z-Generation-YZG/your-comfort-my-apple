
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;

namespace YGZ.Identity.Application.Abstractions.Data;

public interface IGenericRepository<TEntity, TId> where TEntity : class
{
    Task<Result<TEntity>> GetByIdAsync(TId id, Expression<Func<TEntity, object>>[] expressions = null!, CancellationToken? cancellationToken = null);
    Task<Result<List<TEntity>>> GetAllAsync(CancellationToken? cancellationToken = null);
    Task<Result<List<TEntity>>> GetAllByFilterAsync(Expression<Func<TEntity, bool>>? filterExpression = null!, CancellationToken? cancellationToken = null);
    Task<Result<bool>> AddAsync(TEntity entity, CancellationToken? cancellationToken = null);
    Task<Result<bool>> AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken? cancellationToken = null);
    Task<Result<bool>> UpdateAsync(TEntity entity, CancellationToken? cancellationToken = null);
    Task<Result<bool>> UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken? cancellationToken = null);
    Task<Result<bool>> RemoveAsync(TEntity entity, CancellationToken? cancellationToken = null);
    Task<Result<bool>> RemoveRangeAsync(IEnumerable<TEntity> entities, CancellationToken? cancellationToken = null);
}
