using Mapster;
using YGZ.Basket.Api.Contracts;
using YGZ.Basket.Application.ShoppingCarts.Commands.StoreEventItem;

namespace YGZ.Basket.Api.Mappings;

public class StoreEventItemMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<StoreEventItemRequest, StoreEventItemCommand>()
            .Map(dest => dest.EventItemId, src => src.EventItemId)
            .Map(dest => dest.ProductInformation, src => src.ProductInformation);

        config.NewConfig<ProductInformationRequest, ProductInformationCommand>()
            .Map(dest => dest.ProductName, src => src.ProductName)
            .Map(dest => dest.ModelNormalizedName, src => src.ModelNormalizedName)
            .Map(dest => dest.ColorNormalizedName, src => src.ColorNormalizedName)
            .Map(dest => dest.StorageNormalizedName, src => src.StorageNormalizedName)
            .Map(dest => dest.DisplayImageUrl, src => src.DisplayImageUrl);
    }
}
