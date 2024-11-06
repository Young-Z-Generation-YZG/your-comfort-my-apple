

using YGZ.Catalog.Application.Core.Abstractions.Services;
using YGZ.Catalog.Domain.Core.Abstractions.Data;
using YGZ.Catalog.Domain.Core.Abstractions.Result;
using YGZ.Catalog.Domain.Products.Entities;
using YGZ.Catalog.Persistence.Data;

namespace YGZ.Catalog.Persistence.Services;

public class ProductItemService : BaseRepository<ProductItem>, IProductItemService
{

    public ProductItemService(IMongoContext context) : base(context)
    {

    }

    public async Task<Result<ProductItem>> CreateProductItemAsync(ProductItem productItem)
    {
        await Task.CompletedTask;

        Add(productItem);

        return productItem;
    }
}
