
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Discounts;
using YGZ.Discount.Domain.Abstractions.Data;

namespace YGZ.Discount.Application.Coupons.Queries.GetAllPromotionCoupons;

public class GetCouponsHandler : IQueryHandler<GetCouponsQuery, List<CouponResponse>>
{
    private readonly IPromotionCouponRepository _repository;

    public GetCouponsHandler(IPromotionCouponRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<List<CouponResponse>>> Handle(GetCouponsQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
        //var coupons = await _repository.GetAllAsync(cancellationToken);

        //if(coupons is null || coupons.Count == 0)
        //{
        //    return new List<PromotionCouponResponse>();
        //}

        //List<PromotionCouponResponse> responses = MapToResponse(coupons);

        //return responses;
    }

    //private List<PromotionCouponResponse> MapToResponse(List<Coupon> coupons)
    //{
    //    List<PromotionCouponResponse> res = new List<PromotionCouponResponse>();

    //    res.AddRange(coupons.Select(x => new PromotionCouponResponse
    //    {
    //        PromotionCouponId = x.Id.Value.ToString()!,
    //        PromotionCouponTitle = x.Title,
    //        PromotionCouponCode = x.Code.Value,
    //        PromotionCouponDescription = x.Description,
    //        PromotionCouponProductNameTag = x.ProductNameTag.Name,
    //        PromotionCouponPromotionEventType = x.PromotionEventType.Name,
    //        PromotionCouponDiscountState = x.DiscountState.Name,
    //        PromotionCouponDiscountType = x.DiscountType.Name,
    //        PromotionCouponDiscountValue = x.DiscountValue,
    //        PromotionCouponAvailableQuantity = x.AvailableQuantity,
    //        PromotionCouponMaxDiscountAmount = x.MaxDiscountAmount,
    //        PromotionCouponValidFrom = x.ValidFrom is not null ? x.ValidFrom : null,
    //        PromotionCouponValidTo = x.ValidTo is not null ? x.ValidTo : null,
    //    }));

    //    return res;
    //}
}
