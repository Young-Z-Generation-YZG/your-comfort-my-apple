
using YGZ.Catalog.Application.Categories.Commands;
using YGZ.Catalog.Application.ProductItems.Commands;
using YGZ.Catalog.Domain.Categories;
using YGZ.Catalog.Domain.Products.Entities;

namespace YGZ.Catalog.Application.Categories.Extensions;

public static class MappingExtension
{
    public static Category ToEntity(this CreateCategoryCommand dto)
    {
        return Category.Create(name: dto.Name,
                               description: dto.Description,
                               parentId: dto.ParentId);
    }
}
