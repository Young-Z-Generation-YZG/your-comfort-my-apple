
using MongoDB.Bson;
using MongoDB.Driver;
using YGZ.Catalog.Application.Core.Abstractions.Products;
using YGZ.Catalog.Domain.Core.Abstractions.Data;
using YGZ.Catalog.Domain.Core.Abstractions.Result;
using YGZ.Catalog.Domain.Core.Errors;
using YGZ.Catalog.Domain.Products;
using YGZ.Catalog.Domain.Products.Entities;
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

    public async Task<Result<bool>> AddProductItem(string productId, ProductItem productItem)
    {
        var filter = Builders<Product>.Filter.Eq("_id", new ObjectId(productId));
        var update = Builders<Product>.Update.Push("ProductItems", productItem);


        var result = await _collection.UpdateOneAsync(filter, update).ConfigureAwait(false);

        return result.ModifiedCount > 0;
    }
}
