
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;

namespace YGZ.Discount.Application.Coupons.Commands.UpdateCoupon;

public class UpdateCouponCommandHandler : ICommandHandler<UpdateCouponCommand, bool>
{
    public async Task<Result<bool>> Handle(UpdateCouponCommand request, CancellationToken cancellationToken)
    {
        return true;
    }
}
