using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Discounts;
using YGZ.Discount.Grpc.Protos;

namespace YGZ.Catalog.Application.Promotions.Queries.GetPromotionCouponByCode;

public class GetPromotionCouponByCodeQueryHandler : IQueryHandler<GetPromotionCouponByCodeQuery, PromotionCouponResponse>
{
    private readonly DiscountProtoService.DiscountProtoServiceClient _discountProtoServiceClient;

    public GetPromotionCouponByCodeQueryHandler(DiscountProtoService.DiscountProtoServiceClient discountProtoServiceClient)
    {
        _discountProtoServiceClient = discountProtoServiceClient;
    }

    public async Task<Result<PromotionCouponResponse>> Handle(GetPromotionCouponByCodeQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();

        //if (string.IsNullOrEmpty(request.CouponCode))
        //{
        //    return Errors.Coupon.PromotionCodeNotProvided;
        //}

        //var coupon = await _discountProtoServiceClient.GetDiscountByCodeAsync(new GetDiscountRequest { Code = request.CouponCode });

        //if (coupon.PromotionCoupon is null)
        //{
        //    return Errors.Coupon.PromotionCodeDoesNotExist;
        //}

        //if (coupon.PromotionCoupon.PromotionCouponValidTo < DateTime.UtcNow.ToUniversalTime().ToTimestamp())
        //{
        //    return Errors.Coupon.PromotionCodeExpired;
        //}

        //return MapToRepsonse(coupon);
    }

    //private Result<PromotionCouponResponse> MapToRepsonse(CouponResponse coupon)
    //{
    //    return new PromotionCouponResponse
    //    {
    //        PromotionCouponId = coupon.PromotionCoupon.PromotionCouponId,
    //        PromotionCouponTitle = coupon.PromotionCoupon.PromotionCouponTitle,
    //        PromotionCouponCode = coupon.PromotionCoupon.PromotionCouponCode,
    //        PromotionCouponDescription = coupon.PromotionCoupon.PromotionCouponDescription,
    //        PromotionCouponProductNameTag = ProductNameTag.FromValue((int)coupon.PromotionCoupon.PromotionCouponProductNameTag).Name,
    //        PromotionCouponPromotionEventType = PromotionEventType.FromValue((int)coupon.PromotionCoupon.PromotionCouponPromotionEventType).Name,
    //        PromotionCouponDiscountState = DiscountState.FromValue((int)coupon.PromotionCoupon.PromotionCouponDiscountState).Name,
    //        PromotionCouponDiscountType = DiscountType.FromValue((int)coupon.PromotionCoupon.PromotionCouponDiscountType).Name,
    //        PromotionCouponDiscountValue = (decimal)coupon.PromotionCoupon.PromotionCouponDiscountValue!,
    //        PromotionCouponMaxDiscountAmount = coupon.PromotionCoupon.PromotionCouponMaxDiscountAmount.HasValue ? (decimal)coupon.PromotionCoupon.PromotionCouponMaxDiscountAmount : null,
    //        PromotionCouponValidFrom = coupon.PromotionCoupon.PromotionCouponValidFrom.ToDateTime(),
    //        PromotionCouponValidTo = coupon.PromotionCoupon.PromotionCouponValidTo.ToDateTime(),
    //        PromotionCouponAvailableQuantity = (int)coupon.PromotionCoupon.PromotionCouponAvailableQuantity!
    //    };
    //}
}
