using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;

namespace YGZ.Discount.Application.Coupons.Commands.UseCoupon;

public class UseCouponHandler : ICommandHandler<UseCouponCommand, bool>
{
    public async Task<Result<bool>> Handle(UseCouponCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
