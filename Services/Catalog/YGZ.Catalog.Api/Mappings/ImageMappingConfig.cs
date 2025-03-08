using Mapster;
using YGZ.Catalog.Api.Contracts.Common;
using YGZ.Catalog.Application.Common.Commands;

namespace YGZ.Catalog.Api.Mappings;

public class ImageMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.Default.NameMatchingStrategy(NameMatchingStrategy.Flexible);

        config.NewConfig<ImageRequest, ImageCommand>()
            .Map(dest => dest.ImageId, src => src.ImageId)
            .Map(dest => dest.ImageUrl, src => src.ImageUrl)
            .Map(dest => dest.ImageUrl, src => src.ImageUrl);
    }
}
