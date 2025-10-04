
using Mapster;
using YGZ.BuildingBlocks.Shared.Contracts.Common;
using YGZ.BuildingBlocks.Shared.Contracts.ValueObjects;
using YGZ.Catalog.Domain.Products.Common.ValueObjects;

namespace YGZ.Catalog.Application.Common.Mappings;

public class ValueObjectResponseMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.Default.NameMatchingStrategy(NameMatchingStrategy.Flexible);

        config.NewConfig<Color, ColorResponse>()
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.HexCode, src => src.HexCode)
            .Map(dest => dest.ShowcaseImageId, src => src.ShowcaseImageId)
            .Map(dest => dest.Order, src => src.Order);


        config.NewConfig<Storage, StorageResponse>()
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Order, src => src.Order);

        config.NewConfig<Model, ModelResponse>()
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Order, src => src.Order);
    }
}
