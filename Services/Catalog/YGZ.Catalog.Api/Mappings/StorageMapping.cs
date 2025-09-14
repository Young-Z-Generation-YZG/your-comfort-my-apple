using Mapster;
using YGZ.Catalog.Api.Contracts.Common;
using YGZ.Catalog.Application.Common.Commands;

namespace YGZ.Catalog.Api.Mappings;

public class StorageMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.Default.NameMatchingStrategy(NameMatchingStrategy.Flexible);

        config.NewConfig<StorageRequest, StorageCommand>()
            .Map(dest => dest.Name, src => src.StorageName)
            .Map(dest => dest.Value, src => src.StorageValue)
            .Map(dest => dest.Order, src => src.Order);
    }
}
