


using YGZ.Catalog.Application.Products.Commands.CreateProduct;
using YGZ.Catalog.Domain.Core.Abstractions.Result;

namespace YGZ.Catalog.Application.Core.Abstractions.Products;

public interface IProductService
{
    Task<Result<bool>> CreateProductAsync(CreateProductCommand request);
}
