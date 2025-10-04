

using Mapster;
using YGZ.BuildingBlocks.Shared.Contracts.Catalogs;
using YGZ.Catalog.Domain.Categories;

namespace YGZ.Catalog.Application.Common.Mappings;

public class CategoryResponseMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.Default.NameMatchingStrategy(NameMatchingStrategy.Flexible);

        config.NewConfig<Category, CategoryResponse>()
            .Map(dest => dest.CategoryId, src => src.Id.Value)
            .Map(dest => dest.CategoryName, src => src.Name)
            .Map(dest => dest.CategoryDescription, src => src.Description)
            .Map(dest => dest.CategoryOrder, src => src.Order)
            .Map(dest => dest.CategorySlug, src => src.Slug.Value)
            .Map(dest => dest.CategoryParentId, src => src.ParentId.Value);
    }
}
