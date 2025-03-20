

using MongoDB.Driver;

namespace YGZ.Catalog.Domain.Core.Abstractions.Data;

public interface IMongoRepository<TEntity> where TEntity : class
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

    Task InsertOneAsync(TEntity entity);
    Task UpdateAsync(string id, TEntity entity);
    Task DeleteAsync(string id);
}
