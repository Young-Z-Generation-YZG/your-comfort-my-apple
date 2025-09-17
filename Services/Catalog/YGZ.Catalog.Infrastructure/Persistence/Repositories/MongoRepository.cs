using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Catalog.Application.Abstractions.Data;
using YGZ.Catalog.Domain.Core.Abstractions;
using YGZ.Catalog.Domain.Core.Errors;
using YGZ.Catalog.Domain.Core.Primitives;
using YGZ.Catalog.Infrastructure.Persistence.Interceptors;
using YGZ.Catalog.Infrastructure.Settings;

namespace YGZ.Catalog.Infrastructure.Persistence.Repositories;

public class MongoRepository<TEntity, TId> : IMongoRepository<TEntity, TId> where TEntity : Entity<TId> where TId : ValueObject
{
    private readonly ILogger<MongoRepository<TEntity, TId>> _logger;
    private readonly IMongoClient _mongoClient;
    private readonly IMongoCollection<TEntity> _collection;
    private readonly MongoDbSettings _mongoDbSettings;
    private readonly IDispatchDomainEventInterceptor _dispatchDomainEventInterceptor;
    private IClientSessionHandle _session;

    public MongoRepository(IOptions<MongoDbSettings> options, ILogger<MongoRepository<TEntity, TId>> logger, IDispatchDomainEventInterceptor dispatchDomainEventInterceptor)
    {
        _mongoDbSettings = options.Value;

        _mongoClient = new MongoClient(_mongoDbSettings.ConnectionString);
        var database = _mongoClient.GetDatabase(_mongoDbSettings.DatabaseName);

        _collection = database.GetCollection<TEntity>(GetCollectionName(typeof(TEntity)));
        _dispatchDomainEventInterceptor = dispatchDomainEventInterceptor;
        _logger = logger;
    }

    private protected string GetCollectionName(Type documentType)
    {
        var attribute = (BsonCollectionAttribute?)documentType?.GetCustomAttributes(typeof(BsonCollectionAttribute), true)
            .FirstOrDefault();

        return attribute?.CollectionName ?? throw new InvalidOperationException("Collection name attribute is missing.");
    }

    public async Task StartTransactionAsync(CancellationToken? cancellationToken = null)
    {
        _session = await _mongoClient.StartSessionAsync(cancellationToken: cancellationToken ?? CancellationToken.None);
        _session.StartTransaction();

        _logger.LogInformation("Started transaction");
    }

    public async Task RollbackTransaction(IClientSessionHandle session, CancellationToken? cancellationToken = null)
    {
        await session.AbortTransactionAsync(cancellationToken ?? CancellationToken.None);

        _logger.LogInformation("Rolled back transaction");
    }

    public async Task CommitTransaction(IClientSessionHandle session, CancellationToken? cancellationToken = null)
    {
        await session.CommitTransactionAsync(cancellationToken ?? CancellationToken.None);

        _logger.LogInformation("Committed transaction");
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

    public virtual async Task<Result<bool>> InsertOneAsync(TEntity document, IClientSessionHandle? session = null)
    {
        List<IDomainEvent> domainEventEntities = document.DomainEvents.ToList();

        document.ClearDomainEvents();

        foreach (var domainEvent in domainEventEntities)
        {
            await _dispatchDomainEventInterceptor.BeforeInsert(domainEvent);
        }

        try
        {
            if (session != null)
            {
                await _collection.InsertOneAsync(session, document);
            }
            else
            {
                await _collection.InsertOneAsync(document);
            }

            return true;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public virtual async Task<Result<bool>> InsertManyAsync(IEnumerable<TEntity> documents, IClientSessionHandle? session = null)
    {
        try
        {
            await StartTransactionAsync();

            var documentsList = documents.ToList();
            var allDomainEvents = new List<IDomainEvent>();

            foreach (var document in documentsList)
            {
                var domainEventEntities = document.DomainEvents.ToList();
                allDomainEvents.AddRange(domainEventEntities);
                document.ClearDomainEvents();
            }

            foreach (var domainEvent in allDomainEvents)
            {
                await _dispatchDomainEventInterceptor.BeforeInsert(domainEvent);
            }

            var isInTransaction = _session?.IsInTransaction;

            if (_session != null)
            {
                await _collection.InsertManyAsync(_session, documentsList);
            }
            else
            {
                await _collection.InsertManyAsync(documentsList);
            }

            return true;
        }
        catch (Exception ex)
        {
            if (_session != null)
            {
                _logger.LogError(ex, "[Transaction-Rollback] Exception occurred: {Message} {@Exception}", ex.Message, ex);

                await _session.AbortTransactionAsync();
            }

            throw;
        }
    }

    public async Task<Result<bool>> UpdateAsync(string id, TEntity document, IClientSessionHandle? session = null)
    {
        var modifiedCount = 0;
        try
        {
            if (session != null)
            {
                var result = await _collection.ReplaceOneAsync(session, Builders<TEntity>.Filter.Eq("_id", new ObjectId(id)), document);
                modifiedCount = (int)result.ModifiedCount;
            }
            else
            {
                var result = await _collection.ReplaceOneAsync(Builders<TEntity>.Filter.Eq("_id", new ObjectId(id)), document);
                modifiedCount = (int)result.ModifiedCount;
            }

            var domainEventEntities = document.DomainEvents.ToList();

            document.ClearDomainEvents();

            foreach (var domainEvent in domainEventEntities)
            {
                await _dispatchDomainEventInterceptor.BeforeInsert(domainEvent);
            }

            if (modifiedCount > 0)
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
        var deletedCount = 0;
        try
        {
            var result = await _collection.DeleteOneAsync(Builders<TEntity>.Filter.Eq("_id", new ObjectId(id)), cancellationToken);
            deletedCount = (int)result.DeletedCount;

            var domainEventEntities = document.DomainEvents.ToList();

            document.ClearDomainEvents();

            foreach (var domainEvent in domainEventEntities)
            {
                await _dispatchDomainEventInterceptor.BeforeInsert(domainEvent);
            }

            if (deletedCount > 0)
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
