﻿
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using YGZ.Catalog.Application.Core.Abstractions.Products;
using YGZ.Catalog.Application.Products.Commands.CreateProduct;
using YGZ.Catalog.Application.Products.Extensions;
using YGZ.Catalog.Domain.Core.Abstractions.Result;
using YGZ.Catalog.Domain.Core.Errors;
using YGZ.Catalog.Domain.Products.Entities;
using YGZ.Catalog.Persistence.Configurations;

namespace YGZ.Catalog.Persistence.Services;

public class ProductService : IProductService
{
    private readonly ILogger<ProductService> _logger;
    private readonly IMongoCollection<Product> _collection;
    private readonly MongoClient _mongoClient;
    private readonly IMongoDatabase _mongoDb;

    public ProductService(ILogger<ProductService> logger, IOptions<CatalogDbSetting> options)
    {
        _logger = logger;
        _mongoClient = new MongoClient(options.Value.ConnectionString);
        _mongoDb = _mongoClient.GetDatabase(options.Value.DatabaseName);
        _collection = _mongoDb.GetCollection<Product>("Products");
    }

    public async Task<Result<bool>> CreateProductAsync(CreateProductCommand request)
    {
        try
        {
            await _collection.InsertOneAsync(request.ToEntity()).ConfigureAwait(false);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message, nameof(CreateProductAsync));

            return Errors.Product.ProductCannotBeCreated;
        }
    }
}