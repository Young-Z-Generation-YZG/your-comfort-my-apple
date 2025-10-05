

using Mapster;
using YGZ.BuildingBlocks.Shared.Contracts.ValueObjects;
using YGZ.Catalog.Domain.Products.Common.ValueObjects;

namespace YGZ.Catalog.Application.Common.Mappings;

public class ImageResponseMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.Default.NameMatchingStrategy(NameMatchingStrategy.Flexible);

        config.NewConfig<Image, ImageResponse>()
            .Map(dest => dest.ImageId, src => src.ImageId)
            .Map(dest => dest.ImageUrl, src => src.ImageUrl)
            .Map(dest => dest.ImageName, src => src.ImageName)
            .Map(dest => dest.ImageDescription, src => src.ImageDescription)
            .Map(dest => dest.ImageWidth, src => src.ImageWidth)
            .Map(dest => dest.ImageHeight, src => src.ImageHeight)
            .Map(dest => dest.ImageBytes, src => src.ImageBytes)
            .Map(dest => dest.ImageOrder, src => src.ImageOrder);
    }
}
