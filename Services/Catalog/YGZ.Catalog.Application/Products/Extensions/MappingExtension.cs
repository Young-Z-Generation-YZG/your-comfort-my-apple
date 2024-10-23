

using YGZ.Catalog.Application.Products.Commands.CreateProduct;
using YGZ.Catalog.Domain.Products.Entities;

namespace YGZ.Catalog.Application.Products.Extensions;

public static class MappingExtension
{
    public static Product ToEntity(this CreateProductCommand dto)
    {
        return new Product()
        {
            Name = dto.Name
        };
    }
}
