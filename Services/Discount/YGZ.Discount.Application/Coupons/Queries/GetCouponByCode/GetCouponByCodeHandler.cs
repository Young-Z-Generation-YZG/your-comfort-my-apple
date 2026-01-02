using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Discounts;
using YGZ.BuildingBlocks.Shared.Enums;
using YGZ.Discount.Domain.Abstractions.Data;
using YGZ.Discount.Domain.Core.Errors;
using YGZ.Discount.Domain.Coupons;
using YGZ.Discount.Domain.Coupons.ValueObjects;

namespace YGZ.Discount.Application.Coupons.Queries.GetByCouponCode;

public class GetCouponByCodeHandler : IQueryHandler<GetCouponByCodeQuery, CouponResponse>
{
    private readonly ILogger<GetCouponByCodeHandler> _logger;
    private readonly IGenericRepository<Coupon, CouponId> _repository;

    public GetCouponByCodeHandler(IGenericRepository<Coupon, CouponId> repository, ILogger<GetCouponByCodeHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Result<CouponResponse>> Handle(GetCouponByCodeQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(":::[GetCouponByCodeHandler][Query:GetCouponByCodeQuery]::: Method: {MethodName}, Information message: {InformationMessage}, Parameters: {@Parameters}",
            nameof(Handle), "", request);

        var coupon = await _repository.DbSet.FirstOrDefaultAsync(x => x.Code == Code.Of(request.Code), cancellationToken);

        if (coupon is null)
        {
            _logger.LogError(":::[GetCouponByCodeHandler][Result.Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(Handle), Errors.Coupon.NotFound.Message, new { code = request.Code });

            return Errors.Coupon.NotFound;
        }

        if (coupon.IsExpired())
        {
            _logger.LogWarning(":::[GetCouponByCodeHandler][Result.Error]::: Method: {MethodName}, Warning message: {WarningMessage}, Parameters: {@Parameters}",
                nameof(Handle), "Coupon has expired", new { code = request.Code, expiredDate = coupon.ExpiredDate });

            return Errors.Coupon.Expired;
        }

        if (coupon.AvailableQuantity <= 0)
        {
            _logger.LogWarning(":::[GetCouponByCodeHandler][Result.Error]::: Method: {MethodName}, Warning message: {WarningMessage}, Parameters: {@Parameters}",
                nameof(Handle), "Coupon is out of stock", new { code = request.Code, availableQuantity = coupon.AvailableQuantity });

            return Errors.Coupon.OutOfStock;
        }

        if (coupon.DiscountState.Name != EDiscountState.ACTIVE.Name)
        {
            _logger.LogWarning(":::[GetCouponByCodeHandler][Result.Error]::: Method: {MethodName}, Warning message: {WarningMessage}, Parameters: {@Parameters}",
                nameof(Handle), "Coupon is not active", new { code = request.Code, discountState = coupon.DiscountState.Name });

            return Errors.Coupon.Inactive;
        }

        _logger.LogInformation(":::[GetCouponByCodeHandler][Response]::: Method: {MethodName}, Information message: {InformationMessage}, Parameters: {@Parameters}",
            nameof(Handle), "Successfully retrieved and validated coupon by code", coupon.ToResponse());

        return coupon.ToResponse();
    }
}
