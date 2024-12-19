using Mapster;
using YGZ.Basket.Application.Baskets.Commands.CheckoutBasket;
using YGZ.Basket.Application.Baskets.Commands.StoreBasket;
using YGZ.BuildingBlocks.Shared.Contracts.Baskets;

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

        config.NewConfig<CheckoutBasketRequest, CheckoutBasketCommand>()
         .Map(dest => dest, source => source)
         .Map(dest => dest.UserId, source => source.UserId)
         .Map(dest => dest.ContactName, source => source.Contact_name)
         .Map(dest => dest.ContactPhoneNumber, source => source.Contact_phone_number)
         .Map(dest => dest.ContactEmail, source => source.Contact_email)
         .Map(dest => dest.AddressLine, source => source.Address_line)
         .Map(dest => dest.District, source => source.District)
         .Map(dest => dest.Province, source => source.Province)
         .Map(dest => dest.Country, source => source.Country)
         .Map(dest => dest.PaymentType, source => source.Payment_type);

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
