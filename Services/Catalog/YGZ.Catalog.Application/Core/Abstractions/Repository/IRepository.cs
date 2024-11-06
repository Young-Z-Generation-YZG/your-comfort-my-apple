
using MongoDB.Bson;

namespace YGZ.Catalog.Application.Core.Abstractions.Repository;

public interface IRepository<TEntity> : IDisposable where TEntity : class
{
    void Add(TEntity obj);
    Task<TEntity> GetById(Guid id);
    Task<IEnumerable<TEntity>> GetAll();
    void Update(TEntity obj);
    void Remove(ObjectId id);
}
