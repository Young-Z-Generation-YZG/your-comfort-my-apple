using Mapster;
using YGZ.Catalog.Api.Contracts.Common;
using YGZ.Catalog.Application.Common.Commands;

namespace YGZ.Catalog.Api.Mappings;

public class ColorMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.Default.NameMatchingStrategy(NameMatchingStrategy.Flexible);

        config.NewConfig<ColorRequest, ColorCommand>()
            .Map(dest => dest.ColorName, src => src.ColorName)
            .Map(dest => dest.ColorHex, src => src.ColorHex)
            .Map(dest => dest.ColorOrder, src => src.ColorOrder);
    }
}
