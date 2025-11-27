using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using YGZ.BuildingBlocks.Shared.Abstractions.HttpContext;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Constants;
using YGZ.Catalog.Application.Abstractions.Data;
using YGZ.Catalog.Application.Abstractions.Data.Context;
using YGZ.Catalog.Domain.Core.Abstractions;
using YGZ.Catalog.Domain.Core.Primitives;
using YGZ.Catalog.Infrastructure.Persistence.Interceptors;
using YGZ.Catalog.Infrastructure.Settings;

namespace YGZ.Catalog.Infrastructure.Persistence.Repositories;

public class MongoRepository<TEntity, TId> : IMongoRepository<TEntity, TId> where TEntity : Entity<TId> where TId : ValueObject
{
    private readonly ILogger<MongoRepository<TEntity, TId>> _logger;
    private readonly ITenantHttpContext _tenantHttpContext;
    private readonly IUserHttpContext _userHttpContext; 
    private readonly IMongoClient _mongoClient;
    private readonly IMongoCollection<TEntity> _collection;
    private readonly MongoDbSettings _mongoDbSettings;
    private readonly IDispatchDomainEventInterceptor _dispatchDomainEventInterceptor;
    private readonly ITransactionContext _transactionContext;
    private IClientSessionHandle? _session;

    public MongoRepository(IOptions<MongoDbSettings> options,
                           ILogger<MongoRepository<TEntity, TId>> logger,
                           ITenantHttpContext tenantHttpContext,
                           IUserHttpContext userHttpContext,
                           IDispatchDomainEventInterceptor dispatchDomainEventInterceptor,
                           ITransactionContext transactionContext)
    {
        _mongoDbSettings = options.Value;
        _tenantHttpContext = tenantHttpContext;
        _userHttpContext = userHttpContext;
        _mongoClient = new MongoClient(_mongoDbSettings.ConnectionString);
        var database = _mongoClient.GetDatabase(_mongoDbSettings.DatabaseName);

        _collection = database.GetCollection<TEntity>(GetCollectionName(typeof(TEntity)));
        _dispatchDomainEventInterceptor = dispatchDomainEventInterceptor;
        _transactionContext = transactionContext;
        _logger = logger;
    }

    private protected string GetCollectionName(Type documentType)
    {
        var attribute = (BsonCollectionAttribute?)documentType?.GetCustomAttributes(typeof(BsonCollectionAttribute), true)
            .FirstOrDefault();

        return attribute?.CollectionName ?? throw new InvalidOperationException("Collection name attribute is missing.");
    }

    private FilterDefinition<TEntity>? GetBaseTenantFilter()
    {
        // Check if user has ADMIN_SUPER role - if so, skip tenant filtering
        var userRoles = _userHttpContext.GetUserRoles();
        if (userRoles.Contains(AuthorizationConstants.Roles.ADMIN_SUPER))
        {
            _logger.LogInformation("User has ADMIN_SUPER role. Tenant filter will be skipped.");
            return null;
        }

        var tenantId = _tenantHttpContext.GetTenantId();
        if (string.IsNullOrEmpty(tenantId))
        {
            _logger.LogWarning("Tenant ID is not available. Tenant filter will not be applied.");
            return null;
        }

        if (!ObjectId.TryParse(tenantId, out var objectId))
        {
            _logger.LogWarning("Invalid tenant ID format: {TenantId}. Tenant filter will not be applied.", tenantId);
            return null;
        }

        var builder = Builders<TEntity>.Filter;

        var tenantFieldFilter = builder.Eq("tenant_id", objectId);
        var idFieldFilter = builder.Eq("_id", objectId);

        // Some collections store tenant reference under `tenant_id`, while tenant documents use `_id`.
        // Apply both filters via OR so whichever field exists will gate access.
        return builder.Or(tenantFieldFilter, idFieldFilter);
    }

    private FilterDefinition<TEntity> ApplyTenantFilter(FilterDefinition<TEntity>? existingFilter)
    {
        var tenantFilter = GetBaseTenantFilter();
        
        if (tenantFilter == null)
        {
            return existingFilter ?? Builders<TEntity>.Filter.Empty;
        }

        if (existingFilter == null)
        {
            return tenantFilter;
        }

        return Builders<TEntity>.Filter.And(tenantFilter, existingFilter);
    }

    public async Task StartTransactionAsync(CancellationToken? cancellationToken = null)
    {
        _session = await _mongoClient.StartSessionAsync(cancellationToken: cancellationToken ?? CancellationToken.None);
        _session.StartTransaction();

        _transactionContext.SetSession(_session);

        _logger.LogInformation("[Transaction] Started transaction");
    }

    public async Task RollbackTransaction(CancellationToken? cancellationToken = null)
    {
        if (_session == null)
        {
            throw new InvalidOperationException("Transaction not started");
        }

        await _session.AbortTransactionAsync(cancellationToken ?? CancellationToken.None);
        _transactionContext.ClearSession();

        _logger.LogInformation("[Transaction] Rolled back transaction");
    }

    public async Task CommitTransaction(CancellationToken? cancellationToken = null)
    {
        if (_session == null)
        {
            throw new InvalidOperationException("Transaction not started");
        }

        await _session.CommitTransactionAsync(cancellationToken ?? CancellationToken.None);
        _transactionContext.ClearSession();

        _logger.LogInformation("[Transaction] Committed transaction");
    }

    public IClientSessionHandle? GetCurrentSession()
    {
        return _transactionContext.CurrentSession;
    }

    public async Task<List<TEntity>> GetAllAsync(CancellationToken? cancellationToken = null)
    {
        var filter = ApplyTenantFilter(null);
        return await _collection.Find(filter).ToListAsync(cancellationToken ?? CancellationToken.None);
    }

    public async Task<List<TEntity>> GetAllAsync(FilterDefinition<TEntity> filter, CancellationToken? cancellationToken)
    {
        var combinedFilter = ApplyTenantFilter(filter);
        return await _collection.Find(combinedFilter).ToListAsync(cancellationToken ?? CancellationToken.None);
    }

    public async Task<(List<TEntity> items, int totalRecords, int totalPages)> GetAllAsync(int? _page,
                                                                                           int? _limit,
                                                                                           FilterDefinition<TEntity>? filter,
                                                                                           SortDefinition<TEntity>? sort,
                                                                                           CancellationToken? cancellationToken)
    {
        var page = _page ?? 1;
        var limit = _limit ?? 10000;

        var combinedFilter = ApplyTenantFilter(filter);

        var totalRecords = await _collection.CountDocumentsAsync(combinedFilter, cancellationToken: cancellationToken ?? CancellationToken.None);

        var totalPages = (int)Math.Ceiling((double)totalRecords / limit);

        var items = await _collection.Find(combinedFilter)
                                     .Sort(sort)
                                     .Skip((page - 1) * limit)
                                     .Limit(limit)
                                     .ToListAsync(cancellationToken ?? CancellationToken.None);

        return (items, (int)totalRecords, totalPages);
    }

    public async Task<TEntity> GetByIdAsync(string id, CancellationToken? cancellationToken)
    {
        var idFilter = Builders<TEntity>.Filter.Eq("_id", new ObjectId(id));
        var combinedFilter = ApplyTenantFilter(idFilter);
        return await _collection.Find(combinedFilter).FirstOrDefaultAsync(cancellationToken ?? CancellationToken.None);
    }

    public Task<TEntity> GetByFilterAsync(FilterDefinition<TEntity> filter, CancellationToken? cancellationToken)
    {
        var combinedFilter = ApplyTenantFilter(filter);
        return _collection.Find(combinedFilter).FirstOrDefaultAsync(cancellationToken ?? CancellationToken.None);
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
            if (_transactionContext.CurrentSession != null)
            {
                await _collection.InsertOneAsync(_transactionContext.CurrentSession, document);
            }
            else
            {
                await _collection.InsertOneAsync(document);
            }

            return true;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public virtual async Task<Result<bool>> InsertManyAsync(IEnumerable<TEntity> documents)
    {
        try
        {
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

            if (_transactionContext.CurrentSession != null)
            {
                await _collection.InsertManyAsync(_transactionContext.CurrentSession, documentsList);
            }
            else
            {
                await _collection.InsertManyAsync(documentsList);
            }

            return true;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<Result<bool>> UpdateAsync(string id, TEntity document, IClientSessionHandle? session = null)
    {
        var modifiedCount = 0;
        try
        {
            var idFilter = Builders<TEntity>.Filter.Eq("_id", new ObjectId(id));
            var combinedFilter = ApplyTenantFilter(idFilter);

            if (session != null)
            {
                var result = await _collection.ReplaceOneAsync(session, combinedFilter, document);
                modifiedCount = (int)result.ModifiedCount;
            }
            else
            {
                var result = await _collection.ReplaceOneAsync(combinedFilter, document);
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

            return false;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<Result<bool>> DeleteAsync(string id, TEntity document, CancellationToken? cancellationToken)
    {
        var deletedCount = 0;
        try
        {
            var idFilter = Builders<TEntity>.Filter.Eq("_id", new ObjectId(id));
            var combinedFilter = ApplyTenantFilter(idFilter);
            var result = await _collection.DeleteOneAsync(combinedFilter, cancellationToken ?? CancellationToken.None);
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

            return false;
        }
        catch (Exception)
        {
            throw;
        }
    }
}
