using Mapster;
using YGZ.Basket.Application.Baskets.Commands.StoreBasket;
using YGZ.Basket.Application.Contracts;
using YGZ.Basket.Domain.ShoppingCart.Entities;

namespace YGZ.Basket.Api.Common.Mappings;

public class BasketMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<StoreBasketRequest, StoreBasketCommand>()
         .Map(dest => dest, source => source);

        config.NewConfig<StoreBasketRequest, StoreBasketCommand>()
            .Map(dest => dest.CartLines, src => src.Cart_lines); // Handles the list mapping

        config.NewConfig<CartLineRequest, CartLineCommand>()
            .Map(dest => dest, src => src)
            .Map(dest => dest.ImageUrl, src => src.Image_url); // Handles the ImageUrl mapping


    }
}
