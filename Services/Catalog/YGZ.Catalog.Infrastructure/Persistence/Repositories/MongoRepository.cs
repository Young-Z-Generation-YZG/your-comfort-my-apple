using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Catalog.Application.Abstractions.Data;
using YGZ.Catalog.Domain.Core.Errors;
using YGZ.Catalog.Domain.Core.Primitives;
using YGZ.Catalog.Infrastructure.Persistence.Interceptors;
using YGZ.Catalog.Infrastructure.Settings;

namespace YGZ.Catalog.Infrastructure.Persistence.Repositories;

public class MongoRepository<TEntity, TId> : IMongoRepository<TEntity, TId> where TEntity : Entity<TId> where TId : ValueObject
{
    private readonly IMongoCollection<TEntity> _collection;
    private readonly MongoDbSettings _mongoDbSettings;
    private readonly ILogger<MongoRepository<TEntity, TId>> _logger;
    private readonly IDispatchDomainEventInterceptor _dispatchDomainEventInterceptor;

    public MongoRepository(IOptions<MongoDbSettings> options, ILogger<MongoRepository<TEntity, TId>> logger, IDispatchDomainEventInterceptor dispatchDomainEventInterceptor)
    {
        _mongoDbSettings = options.Value;
        var mongoClient = new MongoClient(_mongoDbSettings.ConnectionString);
        var database = mongoClient.GetDatabase(_mongoDbSettings.DatabaseName);
        _collection = database.GetCollection<TEntity>(GetCollectionName(typeof(TEntity)));
        _logger = logger;
        _dispatchDomainEventInterceptor = dispatchDomainEventInterceptor;
    }

    private protected string GetCollectionName(Type documentType)
    {
        var attribute = (BsonCollectionAttribute?)documentType?.GetCustomAttributes(typeof(BsonCollectionAttribute), true)
            .FirstOrDefault();

        return attribute?.CollectionName ?? throw new InvalidOperationException("Collection name attribute is missing.");
    }

    public async Task<List<TEntity>> GetAllAsync()
    {
        return await _collection.Find(new BsonDocument()).ToListAsync();
    }

    public async Task<List<TEntity>> GetAllAsync(FilterDefinition<TEntity> filter, CancellationToken cancellationToken)
    {
        return await _collection.Find(filter).ToListAsync(cancellationToken);
    }

    public async Task<(List<TEntity> items, int totalRecords, int totalPages)> GetAllAsync(int? _page,
                                                                                           int? _limit,
                                                                                           FilterDefinition<TEntity>? filter,
                                                                                           SortDefinition<TEntity>? sort,
                                                                                           CancellationToken cancellationToken)
    {
        var page = _page ?? 1;
        var limit = _limit ?? 10000;

        if (filter == null)
        {
            filter = Builders<TEntity>.Filter.Empty;
        }

        var totalRecords = await _collection.CountDocumentsAsync(filter, cancellationToken: cancellationToken);

        var totalPages = (int)Math.Ceiling((double)totalRecords / limit);

        var items = await _collection.Find(filter)
                                     .Sort(sort)
                                     .Skip((page - 1) * limit)
                                     .Limit(limit)
                                     .ToListAsync(cancellationToken);

        return (items, (int)totalRecords, totalPages);
    }

    public async Task<TEntity> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        return await _collection.Find(Builders<TEntity>.Filter.Eq("_id", new ObjectId(id))).FirstOrDefaultAsync(cancellationToken);
    }

    public Task<TEntity> GetByFilterAsync(FilterDefinition<TEntity> filter, CancellationToken cancellationToken)
    {
        return _collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }

    public virtual async Task<Result<bool>> InsertOneAsync(TEntity document)
    {
        try
        {
            var domainEventEntities = document.DomainEvents.ToList();

            document.ClearDomainEvents();

            foreach (var domainEvent in domainEventEntities)
            {
                await _dispatchDomainEventInterceptor.BeforeInsert(domainEvent);
            }

            await _collection.InsertOneAsync(document);

            return true;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<Result<bool>> UpdateAsync(string id, TEntity document, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _collection.ReplaceOneAsync(Builders<TEntity>.Filter.Eq("_id", new ObjectId(id)), document);

            var domainEventEntities = document.DomainEvents.ToList();

            document.ClearDomainEvents();

            foreach (var domainEvent in domainEventEntities)
            {
                await _dispatchDomainEventInterceptor.BeforeInsert(domainEvent);
            }

            if (result.ModifiedCount > 0)
            {
                return true;
            }

            return Errors.IPhone16Model.UpdatedFailure;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<Result<bool>> DeleteAsync(string id, TEntity document, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _collection.DeleteOneAsync(Builders<TEntity>.Filter.Eq("_id", new ObjectId(id)), cancellationToken);

            var domainEventEntities = document.DomainEvents.ToList();

            document.ClearDomainEvents();

            foreach (var domainEvent in domainEventEntities)
            {
                await _dispatchDomainEventInterceptor.BeforeInsert(domainEvent);
            }

            if (result.DeletedCount > 0)
            {
                return true;
            }

            return Errors.IPhone16Model.UpdatedFailure;
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}
