
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MapsterMapper;
using MediatR;
using YGZ.Discount.Application.Coupons.Commands.CreateCoupon;
using YGZ.Discount.Application.Coupons.Commands.CreatePromotionItem;
using YGZ.Discount.Application.Coupons.Queries.GetByCouponCode;
using YGZ.Discount.Application.PromotionCoupons.Commands.CreatePromotionEvent;
using YGZ.Discount.Application.Promotions.Commands.CreatePromotionCategory;
using YGZ.Discount.Application.Promotions.Commands.CreatePromotionGlobal;
using YGZ.Discount.Application.Promotions.Commands.CreatePromotionProduct;
using YGZ.Discount.Application.Promotions.Queries.GetPromotionGlobal;
using YGZ.Discount.Domain.Core.Enums;
using YGZ.Discount.Grpc.Protos;

namespace YGZ.Discount.Application.Services;

public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
{
    private readonly IMapper _mapper;
    private readonly ISender _sender;

    public DiscountService(IMapper mapper, ISender sender)
    {
        _mapper = mapper;
        _sender = sender;
    }

    public override async Task<CouponResponse> GetDiscountByCode(GetDiscountRequest request, ServerCallContext context)
    {
        var query = new GetCouponByCodeQuery(request.Code);

        var result = await _sender.Send(query);

        var response = _mapper.Map<CouponResponse>(result.Response);

        return response;
    }

    public override async Task<PromotionGlobalResponse> GetPromotionGlobal(GetPromotionGlobalRequest request, ServerCallContext context)
    {
        var query = new GetPromotionGlobalQuery();

        var result = await _sender.Send(query);

        var response = new PromotionGlobalResponse();

        result.Response.ForEach(e =>
        {
            var promotionEventResponse = new PromtionEventResponse();

            promotionEventResponse.PromotionEvent = new PromotionEventModel()
            {
                PromotionEventId = e.promotionEvent.PromotionEventId,
                PromotionEventTitle = e.promotionEvent.PromotionEventTitle,
                PromotionEventDescription = e.promotionEvent.PromotionEventDescription,
                PromotionEventState = (DiscountStateEnum)DiscountState.FromName(e.promotionEvent.PromotionEventState, false).Value,
                PromotionEventValidFrom = e.promotionEvent.PromotionEventValidFrom.HasValue ? e.promotionEvent.PromotionEventValidFrom.Value.ToTimestamp() : null,
                PromotionEventValidTo = e.promotionEvent.PromotionEventValidTo.HasValue ? e.promotionEvent.PromotionEventValidTo.Value.ToTimestamp() : null
            };

            promotionEventResponse.PromotionProducs.AddRange(e.PromotionProducts.Select(p => _mapper.Map<PromotionProductModel>(p)));
            promotionEventResponse.PromotionCategories.AddRange(e.PromotionCategories.Select(c => _mapper.Map<PromotionCategoryModel>(c)));

            response.PromotionGlobals.Add(promotionEventResponse);
        });

        return response;
    }

    //public override async Task<GetAllDiscountsResponse> GetAllDiscounts(GetAllDiscountsRequest request, ServerCallContext context)
    //{
    //    var requestState = (int)request.State;
    //    var state = DiscountState.FromValue(requestState);

    //    var result = await _discountRepository.GetAllAsync(request.Page, request.Limit, state);

    //    var response = new GetAllDiscountsResponse
    //    {
    //        TotalCount = result.TotalCount,
    //        TotalPages = result.TotalPages
    //    };

    //    response.Coupous.AddRange(result.coupons.Select(c => _mapper.Map<CouponModel>(c)));

    //    return response;
    //}

    //public override async Task<BooleanResponse> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    //{
    //    var validFrom = request.CouponModel.ValidFrom.ToDateTime();
    //    var validTo = request.CouponModel.ValidTo.ToDateTime();
    //    var requestType = (int)request.CouponModel.Type;
    //    var requestState = (int)request.CouponModel.State;
    //    var requestNameTag = (int)request.CouponModel.ProductNameTag;
    //    var type = DiscountType.FromValue(requestType);
    //    var state = DiscountState.FromValue(requestState);
    //    var productNameTag = ProductNameTag.FromValue(requestNameTag);
    //    var availableQuantity = request.CouponModel.AvailableQuantity ?? 0;

    //    var coupon = Coupon.Update(couponId: CouponId.Of(Guid.NewGuid()),
    //                               code: Code.Of(request.Code),
    //                               title: request.CouponModel.Title,
    //                               description: request.CouponModel.Description,
    //                               state: state,
    //                               nameTag: productNameTag,
    //                               type: type,
    //                               discountValue: (double)request.CouponModel.DiscountValue!,
    //                               maxDiscountAmount: request.CouponModel.MaxDiscountAmount,
    //                               validFrom: validFrom,
    //                               validTo: validTo,
    //                               availableQuantity: availableQuantity);

    //    var result = await _discountRepository.UpdateAsync(request.Code, coupon);

    //    return new BooleanResponse { IsSuccess = result };
    //}

    //public override async Task<BooleanResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
    //{
    //    var result = await _discountRepository.DeleteAsync(request.Code);

    //    return new BooleanResponse { IsSuccess = result };
    //}

    public override async Task<BooleanResponse> CreatePromotionCoupon(CreatePromotionCouponModel request, ServerCallContext context)
    {
        var cmd = _mapper.Map<CreatePromotionCouponCommand>(request);

        var result = await _sender.Send(cmd);

        var response = _mapper.Map<BooleanResponse>(result.Response);

        return response;
    }

    public override async Task<BooleanResponse> CreatePromotionItem(CreatePromotionItemModelRequest request, ServerCallContext context)
    {
        var cmd = _mapper.Map<CreatePromotionItemCommand>(request);

        var result = await _sender.Send(cmd);

        var response = _mapper.Map<BooleanResponse>(result.Response);

        return response;
    }

    public override async Task<BooleanResponse> CreatePromotionEvent(CreatePromotionEventModelRequest request, ServerCallContext context)
    {
        var cmd = _mapper.Map<CreatePromotionEventCommand>(request);

        var result = await _sender.Send(cmd);

        var response = _mapper.Map<BooleanResponse>(result.Response);

        return response;
    }

    public override async Task<BooleanResponse> CreatePromotionGlobal(PromotionGlobalModelRequest request, ServerCallContext context)
    {
        var cmd = _mapper.Map<CreatePromotionGlobalCommand>(request);

        var result = await _sender.Send(cmd);

        var response = _mapper.Map<BooleanResponse>(result.Response);

        return response;
    }

    public override async Task<BooleanResponse> CreatePromotionProduct(PromotionProductModelRequest request, ServerCallContext context)
    {
        var cmd = new CreatePromotionProductCommand(request.PromotionProductModel.Select(req => _mapper.Map<PromotionProductCommand>(req)).ToList());

        var result = await _sender.Send(cmd);

        var response = _mapper.Map<BooleanResponse>(result.Response);

        return response;
    }

    public override async Task<BooleanResponse> CreatePromotionCategory(PromotionCategoryModelRequest request, ServerCallContext context)
    {
        var cmd = new CreatePromotionCategoryCommand(request.PromotionCategoryModel.Select(req => _mapper.Map<PromotionCategoryCommand>(req)).ToList());

        var result = await _sender.Send(cmd);

        var response = _mapper.Map<BooleanResponse>(result.Response);

        return response;
    }
}
