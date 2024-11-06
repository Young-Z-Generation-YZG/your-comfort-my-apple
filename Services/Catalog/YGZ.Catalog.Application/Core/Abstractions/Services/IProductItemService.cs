

using YGZ.Catalog.Domain.Core.Abstractions.Result;
using YGZ.Catalog.Domain.Products.Entities;

namespace YGZ.Catalog.Application.Core.Abstractions.Services;

public interface IProductItemService
{
    Task<Result<ProductItem>> CreateProductItemAsync(ProductItem request);
}
