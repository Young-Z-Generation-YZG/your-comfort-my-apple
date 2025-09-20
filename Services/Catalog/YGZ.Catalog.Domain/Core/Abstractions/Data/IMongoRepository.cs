

using MongoDB.Driver;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Catalog.Domain.Core.Primitives;

namespace YGZ.Catalog.Domain.Core.Abstractions.Data;

public interface IMongoRepository<TEntity, TId> where TEntity : Entity<TId> where TId : ValueObject
{
    Task<List<TEntity>> GetAllAsync();
    Task<List<TEntity>> GetAllAsync(FilterDefinition<TEntity> filter, CancellationToken cancellationToken);

    Task<(List<TEntity> items, int totalRecords, int totalPages)> GetAllAsync(int? _page,
                                                                              int? _limit,
                                                                              FilterDefinition<TEntity>? filter,
                                                                              SortDefinition<TEntity>? sort,
                                                                              CancellationToken cancellationToken);

    Task<TEntity> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task<TEntity> GetByFilterAsync(FilterDefinition<TEntity> filter, CancellationToken cancellationToken);
    Task StartTransactionAsync(CancellationToken? cancellationToken = null);
    Task RollbackTransaction(CancellationToken? cancellationToken = null);
    Task CommitTransaction(CancellationToken? cancellationToken = null);
    IClientSessionHandle? GetCurrentSession();
    Task<Result<bool>> InsertOneAsync(TEntity document, IClientSessionHandle? session = null);
    Task<Result<bool>> InsertManyAsync(IEnumerable<TEntity> documents);
    Task<Result<bool>> UpdateAsync(string id, TEntity document, IClientSessionHandle? session = null);
    Task<Result<bool>> DeleteAsync(string id, TEntity document, CancellationToken cancellationToken);
}
