
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Discounts;
using YGZ.Discount.Domain.Abstractions.Data;
using YGZ.Discount.Domain.Core.Errors;
using YGZ.Discount.Domain.Coupons;
using YGZ.Discount.Domain.Coupons.ValueObjects;

namespace YGZ.Discount.Application.Coupons.Queries.GetByCouponCode;

public class GetCouponByCodeQueryHandler : IQueryHandler<GetCouponByCodeQuery, GetCouponResponse>
{
    private readonly IPromotionCouponRepository _repository;

    public GetCouponByCodeQueryHandler(IPromotionCouponRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<GetCouponResponse>> Handle(GetCouponByCodeQuery request, CancellationToken cancellationToken)
    {
        var coupon = await _repository.GetByCode(Code.Of(request.Code), cancellationToken);

        if(coupon is null)
        {
            return Errors.Coupon.CouponNotFound;
        }

        GetCouponResponse response = MapToResponse(coupon);

        return response;
    }

    private GetCouponResponse MapToResponse(Coupon coupon)
    {
        return new GetCouponResponse
        {
            PromotionCouponId = coupon.Id.Value.ToString()!,
            PromotionCouponTitle = coupon.Title,
            PromotionCouponCode = coupon.Code.Value,
            PromotionCouponDescription = coupon.Description,
            PromotionCouponProductNameTag = coupon.ProductNameTag.Name,
            PromotionCouponPromotionEventType = coupon.PromotionEventType.Name,
            PromotionCouponDiscountState = coupon.DiscountState.Name,
            PromotionCouponDiscountType = coupon.DiscountType.Name,
            PromotionCouponDiscountValue = coupon.DiscountValue,
            PromotionCouponAvailableQuantity = coupon.AvailableQuantity,
            PromotionCouponMaxDiscountAmount = coupon.MaxDiscountAmount,
            PromotionCouponValidFrom = coupon.ValidFrom is not null ? coupon.ValidFrom : null,
            PromotionCouponValidTo = coupon.ValidTo is not null ? coupon.ValidTo : null,
        };
    }
}
