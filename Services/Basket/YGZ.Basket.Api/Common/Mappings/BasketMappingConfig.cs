using Mapster;
using YGZ.Basket.Api.Contracts;
using YGZ.Basket.Application.Baskets.Commands.CheckoutBasket;
using YGZ.Basket.Application.Baskets.Commands.StoreBasket;
using YGZ.Basket.Application.Contracts;
using YGZ.Basket.Domain.ShoppingCart.Entities;

namespace YGZ.Basket.Api.Common.Mappings;

public class BasketMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {

        config.NewConfig<CartLineRequest, CartLineCommand>()
            .Map(dest => dest.ProductItemId, src => src.ProductItemId)
            .Map(dest => dest.Model, src => src.Model)
            .Map(dest => dest.Color, src => src.Color)
            .Map(dest => dest.Storage, src => src.Storage)
            .Map(dest => dest.Price, src => src.Price)
            .Map(dest => dest.Quantity, src => src.Quantity);

        config.NewConfig<StoreBasketRequest, StoreBasketCommand>()
         .Map(dest => dest, source => source);

        //config.NewConfig<StoreBasketRequest, StoreBasketCommand>()
        //    .Map(dest => dest.CartLines, src => src.Cart_lines); // Handles the list mapping

        //config.NewConfig<CartLineRequest, CartLineCommand>()
        //    .Map(dest => dest, src => src);

        //config.NewConfig<CheckoutBasketRequest, CheckoutBasketCommand>()
        //    .Map(dest => dest, src => src)
        //    .Map(dest => dest.UserId, src => src.UserId)
        //    .Map(dest => dest.FirstName, src => src.First_name)
        //    .Map(dest => dest.LastName, src => src.Last_name);
    }
}
