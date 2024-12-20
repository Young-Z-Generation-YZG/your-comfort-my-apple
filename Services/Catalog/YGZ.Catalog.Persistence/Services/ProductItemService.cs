﻿

using MongoDB.Bson;
using MongoDB.Driver;
using YGZ.Catalog.Application.Core.Abstractions.Services;
using YGZ.Catalog.Domain.Core.Abstractions.Data;
using YGZ.Catalog.Domain.Products.Entities;
using YGZ.Catalog.Persistence.Data;

namespace YGZ.Catalog.Persistence.Services;

public class ProductItemService : BaseRepository<ProductItem>, IProductItemService
{

    public ProductItemService(IMongoContext context) : base(context)
    {

    }

    public override Task InsertOneAsync(ProductItem document, IClientSessionHandle clientSessionHandle, CancellationToken cancellationToken)
    {
        _context.AddCommand(() => _collection.InsertOneAsync(document));

        return Task.CompletedTask;
    }

}
