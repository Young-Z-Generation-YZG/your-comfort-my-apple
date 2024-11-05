


using YGZ.Catalog.Domain.Core.Abstractions.Result;
using YGZ.Catalog.Domain.Products;

namespace YGZ.Catalog.Application.Core.Abstractions.Products;

public interface IProductService
{
    Task<Result<Product>> CreateProductAsync(Product request);
}
