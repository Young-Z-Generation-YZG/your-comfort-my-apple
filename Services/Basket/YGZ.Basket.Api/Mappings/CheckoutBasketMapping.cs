using Mapster;
using YGZ.Basket.Api.Contracts;
using YGZ.Basket.Application.ShoppingCarts.Commands.CheckoutBasket;

namespace YGZ.Basket.Api.Mappings;

public class CheckoutBasketMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.Default.NameMatchingStrategy(NameMatchingStrategy.Flexible);

        config.NewConfig<CheckoutBasketRequest, CheckoutBasketCommand>()
            .Map(dest => dest.ContactName, src => src.ShippingAddress.ContactName)
            .Map(dest => dest.ContactPhoneNumber, src => src.ShippingAddress.ContactPhoneNumber)
            .Map(dest => dest.AddressLine, src => src.ShippingAddress.AddressLine)
            .Map(dest => dest.District, src => src.ShippingAddress.District)
            .Map(dest => dest.Province, src => src.ShippingAddress.Province)
            .Map(dest => dest.Country, src => src.ShippingAddress.Country)
            .Map(dest => dest.PaymentMethod, src => src.PaymentMethod)
            .Map(dest => dest.DiscountCode, src => src.DiscountCode)
            .Map(dest => dest.DiscountAmount, src => src.DiscountAmount)
            .Map(dest => dest.SubTotalAmount, src => src.SubTotalAmount)
            .Map(dest => dest.TotalAmount, src => src.TotalAmount);
    }
}
