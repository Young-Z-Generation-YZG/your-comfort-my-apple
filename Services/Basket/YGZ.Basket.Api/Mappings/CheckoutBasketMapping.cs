using Mapster;
using YGZ.Basket.Api.Contracts;
using YGZ.Basket.Application.ShoppingCarts.Commands.CheckoutBasket;

namespace YGZ.Basket.Api.Mappings;

public class CheckoutBasketMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.Default.NameMatchingStrategy(NameMatchingStrategy.Flexible);

        config.NewConfig<CheckoutBasketRequest, CheckoutBasketCommand>();
        config.NewConfig<ShippingAddressRequest, ShippingAddressCommand>();
    }
}
