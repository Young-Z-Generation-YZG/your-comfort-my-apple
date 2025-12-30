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
            _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(_repository.GetByIdAsync), "Coupon not found", new { couponId = request.CouponId });

            return Errors.Coupon.NotFound;
        }

        if (coupon.AvailableQuantity <= 0)
        {
            _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(Handle), "Coupon is out of stock", new { couponId = request.CouponId, code = coupon.Code.Value, availableQuantity = coupon.AvailableQuantity });

            return Errors.Coupon.OutOfStock;
        }

        coupon.UseCoupon();

        var updateResult = await _repository.UpdateAsync(coupon, cancellationToken);

        if (updateResult.IsFailure)
        {
            _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(_repository.UpdateAsync), "Failed to update coupon after use", new { couponId = request.CouponId, code = coupon.Code.Value, error = updateResult.Error });

            return updateResult.Error;
        }

        _logger.LogInformation("::[Operation Information]:: Method: {MethodName}, Information message: {InformationMessage}, Parameters: {@Parameters}",
            nameof(Handle), "Successfully used coupon", new { couponId = request.CouponId, code = coupon.Code.Value, remainingQuantity = coupon.AvailableQuantity });

        return true;
    }
}
