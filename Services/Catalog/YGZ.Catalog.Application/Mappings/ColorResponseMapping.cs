
using Mapster;
using YGZ.BuildingBlocks.Shared.Contracts.Common;
using YGZ.Catalog.Domain.Products.Common.ValueObjects;

namespace YGZ.Catalog.Application.Mappings;

public class ColorResponseMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.Default.NameMatchingStrategy(NameMatchingStrategy.Flexible);

        config.NewConfig<Color, ColorResponse>()
            .Map(dest => dest.ColorName, src => src.ColorName)
            .Map(dest => dest.ColorHex, src => src.ColorHex)
            .Map(dest => dest.ColorImage, src => src.ColorImage)
            .Map(dest => dest.ColorOrder, src => src.ColorOrder);
    }
}
