using Mapster;
using YGZ.Discount.Grpc.Protos;

namespace YGZ.Discount.Grpc.Mappings;

public class BooleanMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.Default.NameMatchingStrategy(NameMatchingStrategy.Flexible);

        config.NewConfig<bool, BooleanResponse>().Map(dest => dest.IsSuccess, src => src);
    }
}
