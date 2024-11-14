using Mapster;
using YGZ.Catalog.Application.Products.Commands.CreateProduct;
using YGZ.Catalog.Application.Products.Commands.CreateProductItem;
using YGZ.Catalog.Contracts.Products;
using YGZ.Catalog.Domain.Products.Entities;

namespace YGZ.Catalog.Api.Common.Mappings;

public class ProductItemMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.Default.NameMatchingStrategy(NameMatchingStrategy.Flexible);

        config.NewConfig<CreateProductItemRequest, CreateProductItemCommand>()
            .Map(dest => dest, source => source);

        config.NewConfig<ProductItem, CreateProductItemResponse>()
            .Map(dest => dest.Id, source => source.Id.Value)
            .Map(dest => dest.Sku, source => source.Sku.Value)
            .Map(dest => dest.Model, source => source.Model)
            .Map(dest => dest.Color, source => source.Color)
            .Map(dest => dest.Storage, source => source.Storage)
            .Map(dest => dest.Price, source => source.Price)
            .Map(dest => dest.Quantity_in_stock, source => source.QuantityInStock)
            .Map(dest => dest.Images, source => source.Images)
            .Map(dest => dest.Created_at, source => source.CreatedAt)
            .Map(dest => dest.Updated_at, source => source.UpdatedAt);
    }
}
