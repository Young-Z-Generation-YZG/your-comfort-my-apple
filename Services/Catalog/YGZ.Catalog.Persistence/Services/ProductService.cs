
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using YGZ.Catalog.Application.Core.Abstractions.Products;
using YGZ.Catalog.Domain.Core.Abstractions.Data;
using YGZ.Catalog.Domain.Core.Abstractions.Result;
using YGZ.Catalog.Domain.Core.Errors;
using YGZ.Catalog.Domain.Products;
using YGZ.Catalog.Persistence.Configurations;
using YGZ.Catalog.Persistence.Data;

namespace YGZ.Catalog.Persistence.Services;

public class ProductService : BaseRepository<Product>, IProductService
{
    public ProductService(IMongoContext context) : base(context)
    {
    }

    public async Task<Result<Product>> CreateProductAsync(Product product)
    {
        await Task.CompletedTask;

        _context.AddCommand(() => _collection.InsertOneAsync(product));

        return product;
    }
}
