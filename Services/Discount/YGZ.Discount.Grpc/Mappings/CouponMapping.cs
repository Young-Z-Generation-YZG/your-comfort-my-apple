using Mapster;
using YGZ.Discount.Application.Coupons.Commands.CreateCoupon;
using YGZ.Discount.Grpc.Protos;

namespace YGZ.Discount.Grpc.Mappings;

public class CouponMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.Default.NameMatchingStrategy(NameMatchingStrategy.Flexible);

        config.NewConfig<CreateCouponRequest, CreateCouponCommand>();
    }
}
