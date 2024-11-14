
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace YGZ.Catalog.Application.Core.Abstractions.Repository;

public interface IRepository<TEntity> : ISession, IDisposable where TEntity : class
{
    void Add(TEntity obj);
    Task<TEntity> GetById(ObjectId id);
    Task<IEnumerable<TEntity>> GetAll();
    void Update(TEntity obj);
    void Remove(ObjectId id);

    IQueryable<TEntity> AsQueryable();

    Task<IEnumerable<TEntity>> FilterByAsync(
        Expression<Func<TEntity, bool>> filterExpression,
        CancellationToken cancellationToken);

    Task<IEnumerable<TProjected>> FilterByAsync<TProjected>(
        Expression<Func<TEntity, bool>> filterExpression,
        FindOptions<TEntity, TProjected> projectionExpression,
        CancellationToken cancellationToken);

    Task<TEntity> FindOneAsync(
        Expression<Func<TEntity, bool>> filterExpression,
        CancellationToken cancellationToken);

    Task<TEntity> FindByIdAsync(
        string id,
        CancellationToken cancellationToken);

    Task InsertOneAsync(
        TEntity document,
        IClientSessionHandle clientSessionHandle,
        CancellationToken cancellationToken);

    Task InsertManyAsync(
        ICollection<TEntity> documents,
        IClientSessionHandle clientSessionHandle,
        CancellationToken cancellationToken);

    Task<bool> ReplaceOneAsync(
        TEntity document,
        IClientSessionHandle clientSessionHandle,
        CancellationToken cancellationToken);

    Task<bool> DeleteOneAsync(
        string id,
        IClientSessionHandle clientSessionHandle,
        CancellationToken cancellationToken);

    Task<bool> DeleteManyAsync(
        Expression<Func<TEntity,
        bool>> filterExpression,
        IClientSessionHandle clientSessionHandle,
        CancellationToken cancellationToken);
}
