

using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq.Expressions;
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
        _collection = _context.GetCollection<TEntity>(typeof(TEntity).Name + 's');
    }

    public virtual void Add(TEntity obj)
    {
        _context.AddCommand(() => _collection.InsertOneAsync(obj));
    }

    public virtual async Task<TEntity> GetById(ObjectId id)
    {
        var filter = Builders<TEntity>.Filter.Eq("_id", id.ToString());
        var result = await _collection.Find(filter).FirstOrDefaultAsync();

        return result;
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

    public virtual IQueryable<TEntity> AsQueryable()
    {
        return _collection.AsQueryable();
    }

    public virtual async Task<TEntity> FindByIdAsync(string id, CancellationToken cancellationToken)
    {
        var objectId = new ObjectId(id);
        var filter = Builders<TEntity>.Filter.Eq("_id", objectId);
        return await (await _collection.FindAsync(filter, null, cancellationToken)).SingleOrDefaultAsync(cancellationToken);
    }

    public Task<TEntity> FindOneAsync(Expression<Func<TEntity, bool>> filterExpression, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<TEntity>> FilterByAsync(Expression<Func<TEntity, bool>> filterExpression, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<TProjected>> FilterByAsync<TProjected>(Expression<Func<TEntity, bool>> filterExpression, FindOptions<TEntity, TProjected> projectionExpression, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public virtual async Task InsertOneAsync(TEntity document, IClientSessionHandle clientSessionHandle, CancellationToken cancellationToken)
    {
        await _collection.InsertOneAsync(clientSessionHandle, document, null, cancellationToken);
    }

    public Task InsertManyAsync(ICollection<TEntity> documents, IClientSessionHandle clientSessionHandle, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ReplaceOneAsync(TEntity document, IClientSessionHandle clientSessionHandle, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteOneAsync(string id, IClientSessionHandle clientSessionHandle, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteManyAsync(Expression<Func<TEntity, bool>> filterExpression, IClientSessionHandle clientSessionHandle, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public virtual async Task<IClientSessionHandle> BeginSessionAsync(CancellationToken cancellationToken)
    {
        var option = new ClientSessionOptions();
        option.DefaultTransactionOptions = new TransactionOptions();
        return await _collection.Database.Client.StartSessionAsync(option, cancellationToken);
    }

    public virtual void BeginTransaction(IClientSessionHandle clientSessionHandle)
        => clientSessionHandle.StartTransaction();

    public virtual Task CommitTransactionAsync(IClientSessionHandle clientSessionHandle, CancellationToken cancellationToken)
    => clientSessionHandle.CommitTransactionAsync(cancellationToken);

    public virtual Task RollbackTransactionAsync(IClientSessionHandle clientSessionHandle, CancellationToken cancellationToken)
    => clientSessionHandle.AbortTransactionAsync(cancellationToken);

    public virtual void DisposeSession(IClientSessionHandle clientSessionHandle)
    => clientSessionHandle.Dispose();
}
