


using YGZ.Catalog.Application.Products.Commands.CreateProduct;
using YGZ.Catalog.Domain.Core.Abstractions.Result;
using YGZ.Catalog.Domain.Products.Entities;

namespace YGZ.Catalog.Application.Core.Abstractions.Products;

public interface IProductService
{
    Task<Result<bool>> CreateProductAsync(CreateProductCommand request);
}
