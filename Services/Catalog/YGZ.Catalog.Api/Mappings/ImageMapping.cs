using Mapster;
using YGZ.Catalog.Api.Contracts.Common;
using YGZ.Catalog.Application.Common.Commands;

namespace YGZ.Catalog.Api.Mappings;

public class ImageMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.Default.NameMatchingStrategy(NameMatchingStrategy.Flexible);

        config.NewConfig<ImageRequest, ImageCommand>()
            .Map(dest => dest.ImageId, src => src.ImageId)
            .Map(dest => dest.ImageUrl, src => src.ImageUrl)
            .Map(dest => dest.ImageName, src => src.ImageName)
            .Map(dest => dest.ImageDescription, src => src.ImageDescription)
            .Map(dest => dest.ImageWidth, src => src.ImageWidth)
            .Map(dest => dest.ImageHeight, src => src.ImageHeight)
            .Map(dest => dest.ImageBytes, src => src.ImageBytes)
            .Map(dest => dest.Order, src => src.Order);
    }
}
