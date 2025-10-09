using Mapster;
using YGZ.Basket.Api.Contracts;
using YGZ.Basket.Application.ShoppingCarts.Commands.StoreEventItem;

namespace YGZ.Basket.Api.Mappings;

public class StoreEventItemMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<StoreEventItemRequest, StoreEventItemCommand>()
            .Map(dest => dest.EventItemId, src => src.EventItemId);
    }
}
