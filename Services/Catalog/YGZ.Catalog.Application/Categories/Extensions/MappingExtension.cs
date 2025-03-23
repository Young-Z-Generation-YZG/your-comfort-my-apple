
using YGZ.Catalog.Application.Categories.Commands;
using YGZ.Catalog.Domain.Categories;
using YGZ.Catalog.Domain.Categories.ValueObjects;

namespace YGZ.Catalog.Application.Categories.Extensions;

public static class MappingExtension
{
    public static Category ToEntity(this CreateCategoryCommand dto)
    {
        return Category.Create(id: CategoryId.Create(), name: dto.Name,
                               description: dto.Description,
                               order: dto.Order,
                               parentId: CategoryId.Of(dto.ParentId));
    }
}
