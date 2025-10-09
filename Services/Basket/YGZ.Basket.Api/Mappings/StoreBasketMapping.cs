using Mapster;

namespace YGZ.Basket.Api.Mappings;

public class StoreBasketMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.Default.NameMatchingStrategy(NameMatchingStrategy.Flexible);
    }
}
