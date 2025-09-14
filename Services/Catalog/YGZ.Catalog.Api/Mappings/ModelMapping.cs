using Mapster;
using YGZ.Catalog.Api.Contracts.Common;
using YGZ.Catalog.Application.Common.Commands;

namespace YGZ.Catalog.Api.Mappings;

public class ModelMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.Default.NameMatchingStrategy(NameMatchingStrategy.Flexible);

        config.NewConfig<ModelRequest, IPhoneModelCommand>()
            .Map(dest => dest.Name, src => src.ModelName)
            .Map(dest => dest.Order, src => src.Order);
    }
}
