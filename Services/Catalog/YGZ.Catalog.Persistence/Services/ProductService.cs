
using MongoDB.Bson;
using MongoDB.Driver;
using YGZ.Catalog.Application.Core.Abstractions.Products;
using YGZ.Catalog.Domain.Core.Abstractions.Data;
using YGZ.Catalog.Domain.Core.Abstractions.Result;
using YGZ.Catalog.Domain.Core.Errors;
using YGZ.Catalog.Domain.Products;
using YGZ.Catalog.Persistence.Data;

namespace YGZ.Catalog.Persistence.Services;

public class ProductService : ProductRepository, IProductService
{
    public ProductService(IMongoContext context) : base(context){}

    public override Task InsertOneAsync(Product document, IClientSessionHandle? clientSessionHandle, CancellationToken cancellationToken)
    {
        _context.AddCommand(() => _collection.InsertOneAsync(document));

        return Task.CompletedTask;
    }

    //public async Task<Result<Product>> GetByIdAsync(string id)
    //{
    //    try
    //    {
    //        var objectId = ObjectId.Parse(id);

    //        var filter = Builders<Product>.Filter.Eq("_id", objectId);
    //        var result = await _collection.Find(filter).SingleOrDefaultAsync();

    //        var test = await GetById(objectId);

    //        return test;
    //    }
    //    catch
    //    {
    //        return Errors.Product.IdInvalid;
    //    }
    //}
}
