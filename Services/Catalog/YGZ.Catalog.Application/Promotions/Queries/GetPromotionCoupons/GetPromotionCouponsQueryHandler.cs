

using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Common;
using YGZ.BuildingBlocks.Shared.Contracts.Discounts;
using YGZ.Discount.Grpc.Protos;

namespace YGZ.Catalog.Application.Promotions.Queries.GetPromotionCoupons;

public class GetPromotionCouponsQueryHandler : IQueryHandler<GetPromotionCouponsQuery, PaginationResponse<CouponResponse>>
{
    private readonly DiscountProtoService.DiscountProtoServiceClient _discountProtoServiceClient;

    public GetPromotionCouponsQueryHandler(DiscountProtoService.DiscountProtoServiceClient discountProtoServiceClient)
    {
        _discountProtoServiceClient = discountProtoServiceClient;
    }

    public async Task<Result<PaginationResponse<CouponResponse>>> Handle(GetPromotionCouponsQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
        //var promotionCoupons = await _discountProtoServiceClient.GetAllPromotionCouponsAsync(new GetAllPromotionCouponsRequest());

        //if(promotionCoupons is null || promotionCoupons.PromotionCoupons.Count == 0)
        //{
        //    return new PaginationResponse<PromotionCouponResponse>();
        //}

        //PaginationResponse<PromotionCouponResponse> res = MapToResponse(promotionCoupons, request);

        //return res;
    }

    //private PaginationResponse<PromotionCouponResponse> MapToResponse(GetAllDiscountsResponse promotionCoupons, GetPromotionCouponsQuery request)
    //{
    //    PaginationResponse<PromotionCouponResponse> res = new PaginationResponse<PromotionCouponResponse>();

    //    res.TotalRecords = 0;
    //    res.TotalPages = 0;
    //    res.PageSize = 0;
    //    res.CurrentPage = 0;

    //    var queryParams = QueryParamBuilder.Build(request);

    //    PaginationLinks paginationLinks = PaginationLinksBuilder.Build(basePath: "", queryParams: queryParams, currentPage: 1, totalPages: 1);
    //    res.Links = paginationLinks;

    //    res.Items = promotionCoupons.PromotionCoupons.Select(x => new PromotionCouponResponse
    //    {
    //        PromotionCouponId = x.PromotionCouponId,
    //        PromotionCouponTitle = x.PromotionCouponTitle,
    //        PromotionCouponCode = x.PromotionCouponCode,
    //        PromotionCouponDescription = x.PromotionCouponDescription,
    //        PromotionCouponProductNameTag = ProductNameTag.FromValue((int)x.PromotionCouponProductNameTag).Name,
    //        PromotionCouponPromotionEventType = PromotionEventType.FromValue((int)x.PromotionCouponPromotionEventType).Name,
    //        PromotionCouponDiscountState = DiscountState.FromValue((int)x.PromotionCouponDiscountState).Name,
    //        PromotionCouponDiscountType = DiscountType.FromValue((int)x.PromotionCouponDiscountType).Name,
    //        PromotionCouponDiscountValue = (decimal)x.PromotionCouponDiscountValue!,
    //        PromotionCouponMaxDiscountAmount = x.PromotionCouponMaxDiscountAmount.HasValue ? (decimal)x.PromotionCouponMaxDiscountAmount : null,
    //        PromotionCouponValidFrom = x.PromotionCouponValidFrom.ToDateTime(),
    //        PromotionCouponValidTo = x.PromotionCouponValidTo.ToDateTime(),
    //        PromotionCouponAvailableQuantity = (int)x.PromotionCouponAvailableQuantity!
    //    }).ToList();

    //    return res;
    //}
}
