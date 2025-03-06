

using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using YGZ.Catalog.Domain.Core.Abstractions.Data;
using YGZ.Catalog.Domain.Core.Primitives;
using YGZ.Catalog.Infrastructure.Settings;

namespace YGZ.Catalog.Infrastructure.Persistence;

public class MongoRepository<TEntity> : IMongoRepository<TEntity> where TEntity : class
{
    private readonly IMongoCollection<TEntity> _collection;
    private readonly MongoDbSettings _mongoDbSettings;

    public MongoRepository(IOptions<MongoDbSettings> options)
    {
        _mongoDbSettings = options.Value;

        var database = new MongoClient(_mongoDbSettings.ConnectionString).GetDatabase(_mongoDbSettings.DatabaseName);

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


    public async Task<TEntity> GetByIdAsync(string id)
    {
        return await _collection.Find(Builders<TEntity>.Filter.Eq("_id", new ObjectId(id))).FirstOrDefaultAsync();
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
