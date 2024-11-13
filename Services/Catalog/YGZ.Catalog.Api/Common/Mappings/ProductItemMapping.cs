using Mapster;
using YGZ.Catalog.Application.Products.Commands.CreateProduct;
using YGZ.Catalog.Application.Products.Commands.CreateProductItem;
using YGZ.Catalog.Contracts.Products;
using YGZ.Catalog.Domain.Products.Entities;

namespace YGZ.Catalog.Api.Common.Mappings;

public class ProductItemMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.Default.NameMatchingStrategy(NameMatchingStrategy.Flexible);

        config.NewConfig<CreateProductItemRequest, CreateProductItemCommand>()
            .Map(dest => dest, source => source);

        //config.NewConfig<CreateProductItemRequest, CreateProductCommand>()
        //    .Map(dest => dest, source => source);

        //config.NewConfig<Product, CreateProductResponse>()
        //    .Map(dest => dest.Id, source => source.Id.Value)
        //    .Map(dest => dest.Slug, source => source.Slug.Value)
        //    .Map(dest => dest.CategoryId, source => source.CategoryId.Value)
        //    .Map(dest => dest.PromotionId, source => source.PromotionId.Value)
        //    .Map(dest => dest.Product_items, source => source.ProductItems)
        //    .Map(dest => dest, src => src);

        //config.NewConfig<ProductItem, ProductItemResponse>()
        //    .Map(dest => dest.Id, source => source.Id.Value)
        //    .Map(dest => dest.Sku, source => source.Sku.Value);
    }
}
