

using MongoDB.Bson;
using MongoDB.Driver;
using YGZ.Catalog.Application.Core.Abstractions.Repository;
using YGZ.Catalog.Domain.Core.Abstractions.Data;

namespace YGZ.Catalog.Persistence.Data;

public abstract class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class
{
    protected readonly IMongoContext _context;
    protected readonly IMongoCollection<TEntity> _collection;

    protected BaseRepository(IMongoContext context)
    {
        _context = context;
        _collection = _context.GetCollection<TEntity>(typeof(TEntity).Name + "s");
    }

    public virtual void Add(TEntity obj)
    {
        _context.AddCommand(() => _collection.InsertOneAsync(obj));
    }

    public virtual async Task<TEntity> GetById(Guid id)
    {
        var data = await _collection.FindAsync(Builders<TEntity>.Filter.Eq("_id", id));
        return data.SingleOrDefault();
    }

    public virtual async Task<IEnumerable<TEntity>> GetAll()
    {
        var all = await _collection.FindAsync(Builders<TEntity>.Filter.Empty);
        return all.ToList();
    }

    public virtual void Update(TEntity obj)
    {
        throw new NotImplementedException();
    }

    public virtual void Remove(ObjectId id)
    {
        _context.AddCommand(() => _collection.DeleteOneAsync(Builders<TEntity>.Filter.Eq("_id", id)));
    }

    public void Dispose()
    {
        _context?.Dispose();
    }
}
