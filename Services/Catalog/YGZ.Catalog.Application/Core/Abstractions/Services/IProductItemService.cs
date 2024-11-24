

using YGZ.Catalog.Application.Core.Abstractions.Repository;
using YGZ.Catalog.Domain.Core.Abstractions.Result;
using YGZ.Catalog.Domain.Products.Entities;

namespace YGZ.Catalog.Application.Core.Abstractions.Services;

public interface IProductItemService : IRepository<ProductItem>
{
    //Task<Result<ProductItem>> GetByIdAsync(string id);
    //void CreateAsync(ProductItem productItem);
}
