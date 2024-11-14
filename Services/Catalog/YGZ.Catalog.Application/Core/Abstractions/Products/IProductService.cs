


using YGZ.Catalog.Application.Core.Abstractions.Repository;
using YGZ.Catalog.Domain.Core.Abstractions.Result;
using YGZ.Catalog.Domain.Products;
using YGZ.Catalog.Domain.Products.Entities;

namespace YGZ.Catalog.Application.Core.Abstractions.Products;

public interface IProductService : IRepository<Product>
{
    Task<Result<bool>> AddProductItem(string productId, ProductItem productItem);
}
