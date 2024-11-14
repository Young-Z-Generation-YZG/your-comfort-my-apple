using Mapster;
using YGZ.Catalog.Application.Products.Commands.CreateProduct;
using YGZ.Catalog.Contracts.Products;
using YGZ.Catalog.Domain.Products;

namespace YGZ.Catalog.Api.Common.Mappings;

public class ProductMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.Default.NameMatchingStrategy(NameMatchingStrategy.Flexible);

        config.NewConfig<CreateProductItemRequest, CreateProductCommand>()
            .Map(dest => dest, source => source);


        config.NewConfig<Product, CreateProductResponse>()
            .Map(dest => dest.Id, source => source.Id.Value)
            .Map(dest => dest.Slug, source => source.Slug.Value)
            .Map(dest => dest.CategoryId, source => source.CategoryId.Value)
            .Map(dest => dest.PromotionId, source => source.PromotionId.Value);

        //config.NewConfig<ProductItem, ProductItemResponse>()
        //    .Map(dest => dest.Id, source => source.Id.Value)
        //    .Map(dest => dest.Sku, source => source.Sku.Value);
    }
}
