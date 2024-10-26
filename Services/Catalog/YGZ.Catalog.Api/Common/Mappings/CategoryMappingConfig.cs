using Mapster;
using YGZ.Catalog.Application.Categories.Commands.CreateCategory;
using YGZ.Catalog.Contracts.Categories;

namespace YGZ.Catalog.Api.Common.Mappings;

public class CategoryMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateCategoryRequest, CreateCategoryCommand>();
    }
}
