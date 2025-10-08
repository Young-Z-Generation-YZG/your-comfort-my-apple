using Mapster;

namespace YGZ.Basket.Api.Mappings;

public class StoreBasketMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.Default.NameMatchingStrategy(NameMatchingStrategy.Flexible);

        //config.NewConfig<StoreBasketRequest, StoreBasketCommand>()
        //    .Map(dest => dest.CartItems, src => src.CartItems);

        //config.NewConfig<CartItemRequest, CartItemCommand>()
        //    .Map(dest => dest.ProductId, src => src.ProductId)
        //    .Map(dest => dest.ModelId, src => src.ModelId)
        //    .Map(dest => dest.ProductName, src => src.ProductName)
        //    .Map(dest => dest.ProductColorName, src => src.ProductColorName)
        //    .Map(dest => dest.ProductUnitPrice, src => src.ProductUnitPrice)
        //    .Map(dest => dest.ProductNameTag, src => src.ProductNameTag)
        //    .Map(dest => dest.ProductImage, src => src.ProductImage)
        //    .Map(dest => dest.ProductSlug, src => src.ProductSlug)
        //    .Map(dest => dest.CategoryId, src => src.CategoryId)
        //    .Map(dest => dest.Quantity, src => src.Quantity)
        //    .Map(dest => dest.Promotion, src => src.Promotion);

        //config.NewConfig<PromotionRequest, PromotionCommand>()
        //    .Map(dest => dest.PromotionIdOrCode, src => src.PromotionIdOrCode)
        //    .Map(dest => dest.PromotionEventType, src => src.PromotionEventType);
    }
}
