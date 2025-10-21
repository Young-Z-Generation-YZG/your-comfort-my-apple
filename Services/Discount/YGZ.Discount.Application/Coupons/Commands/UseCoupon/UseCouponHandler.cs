using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Discount.Domain.Abstractions.Data;
using YGZ.Discount.Domain.Core.Errors;
using YGZ.Discount.Domain.Coupons;
using YGZ.Discount.Domain.Coupons.ValueObjects;

namespace YGZ.Discount.Application.Coupons.Commands.UpdateCouponQuantity;

public class UseCouponHandler : ICommandHandler<UseCouponCommand, bool>
{
    private readonly IGenericRepository<Coupon, CouponId> _repository;
    private readonly ILogger<UseCouponHandler> _logger;

    public UseCouponHandler(IGenericRepository<Coupon, CouponId> repository, ILogger<UseCouponHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Result<bool>> Handle(UseCouponCommand request, CancellationToken cancellationToken)
    {
        var coupon = await _repository.GetByIdAsync(CouponId.Of(request.CouponId), cancellationToken);

        if (coupon is null)
        {
            return Errors.Coupon.NotFound;
        }

        if (coupon.AvailableQuantity <= 0)
        {
            return Errors.Coupon.OutOfStock;
        }

        coupon.UseCoupon();

        await _repository.UpdateAsync(coupon, cancellationToken);

        return true;
    }
}
