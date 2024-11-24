using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Mapster;
using YGZ.Catalog.Contracts.Common;

namespace YGZ.Catalog.Api.Common.Mappings;

public class ImageMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {

        config.NewConfig<ImageUploadResult, UploadImageResponse>()
            .Map(dest => dest, source => source)
            .Map(dest => dest.Original_filename, source => source.OriginalFilename);

        config.NewConfig<Resource, UploadImageResponse>()
            .Map(dest => dest.Public_id, source => source.PublicId)
            .Map(dest => dest.Original_filename, source => source.Format)
            .Map(dest => dest.Format, source => source.Format)
            .Map(dest => dest.Secure_url, source => source.SecureUrl)
            .Map(dest => dest.Width, source => (decimal)source.Width)
            .Map(dest => dest.Height, source => (decimal)source.Height)
            .Map(dest => dest.Length, source => (decimal)source.Length)
            .Map(dest => dest.Bytes, source => (decimal)source.Bytes)
            .Map(dest => dest, source => source);

        config.NewConfig<ListResourcesResult, List<UploadImageResponse>>()
            .Map(dest => dest, src => src.Resources);

    }
}
