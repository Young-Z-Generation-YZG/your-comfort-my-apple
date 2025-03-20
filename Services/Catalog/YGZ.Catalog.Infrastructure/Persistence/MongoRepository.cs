

using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using YGZ.Catalog.Domain.Core.Abstractions.Data;
using YGZ.Catalog.Domain.Core.Primitives;
using YGZ.Catalog.Infrastructure.Settings;
using Polly;

namespace YGZ.Catalog.Infrastructure.Persistence;

public class MongoRepository<TEntity> : IMongoRepository<TEntity> where TEntity : class
{
    private readonly IMongoCollection<TEntity> _collection;
    private readonly MongoDbSettings _mongoDbSettings;

    public MongoRepository(IOptions<MongoDbSettings> options)
    {
        _mongoDbSettings = options.Value;

        var mongoClient = new MongoClient(_mongoDbSettings.ConnectionString);
      
        var database = mongoClient.GetDatabase(_mongoDbSettings.DatabaseName);

        _collection = database.GetCollection<TEntity>(GetCollectionName(typeof(TEntity)));
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
        var limit = _limit ?? 10;

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

    public async Task InsertOneAsync(TEntity document)
    {
        await _collection.InsertOneAsync(document);

    }

    public async Task UpdateAsync(string id, TEntity entity)
    {
        await _collection.ReplaceOneAsync(Builders<TEntity>.Filter.Eq("_id", new ObjectId(id)), entity);
    }

    public async Task DeleteAsync(string id)
    {
        await _collection.DeleteOneAsync(Builders<TEntity>.Filter.Eq("_id", new ObjectId(id)));
    }
}
