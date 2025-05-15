using CloudinaryDotNet.Actions;
using Mapster;
using YGZ.BuildingBlocks.Shared.Contracts.Common;

namespace YGZ.Catalog.Api.Mappings;

public class ImageMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {

        config.NewConfig<ImageUploadResult, CloudinaryImageResponse>()
            .Map(dest => dest, source => source)
            .Map(dest => dest.OriginalFilename, source => source.OriginalFilename);

        config.NewConfig<Resource, CloudinaryImageResponse>()
            .Map(dest => dest.OriginalFilename, source => source.Format)
            .Map(dest => dest.PublicId, source => source.PublicId)
            .Map(dest => dest.Format, source => source.Format)
            .Map(dest => dest.SecureUrl, source => source.SecureUrl)
            .Map(dest => dest.Width, source => (decimal)source.Width)
            .Map(dest => dest.Height, source => (decimal)source.Height)
            .Map(dest => dest.Length, source => (decimal)source.Bytes)
            .Map(dest => dest.Bytes, source => (decimal)source.Bytes)
            .Map(dest => dest, source => source);

        config.NewConfig<ListResourcesResult, List<CloudinaryImageResponse>>()
            .Map(dest => dest, src => src.Resources);

    }
}
