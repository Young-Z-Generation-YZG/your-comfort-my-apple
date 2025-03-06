

namespace YGZ.Catalog.Domain.Core.Abstractions.Data;

public interface IMongoRepository<TEntity> where TEntity : class
{
    Task InsertOneAsync(TEntity entity);
    Task<List<TEntity>> GetAllAsync();
    Task<TEntity> GetByIdAsync(string id);
    Task UpdateAsync(string id, TEntity entity);
    Task DeleteAsync(string id);
}
