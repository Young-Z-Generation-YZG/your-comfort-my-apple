
using Mapster;
using YGZ.BuildingBlocks.Shared.Contracts.Catalogs.WithPromotion;
using YGZ.Catalog.Domain.Products.Iphone16.Entities;

namespace YGZ.Catalog.Application.Mappings;

public class IPhone16ResponseMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.Default.NameMatchingStrategy(NameMatchingStrategy.Flexible);

        config.NewConfig<IPhone16Detail, IPhoneDetailsWithPromotionResponse>()
            .Map(dest => dest.ProductId, src => src.Id.Value)
            .Map(dest => dest.ProductModel, src => src.Model)
            .Map(dest => dest.ProductStorage, src => src.Storage)
            .Map(dest => dest.ProductDescription, src => src.Description)
            .Map(dest => dest.ProductUnitPrice, src => src.UnitPrice)
            .Map(dest => dest.ProductAvailableInStock, src => src.AvailableInStock)
            .Map(dest => dest.ProductSlug, src => src.Slug.Value);

    }
}
