
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MapsterMapper;
using MediatR;
using YGZ.BuildingBlocks.Shared.Enums;
using YGZ.Discount.Application.Coupons.Commands.CreateCoupon;
using YGZ.Discount.Application.Coupons.Commands.CreatePromotionItem;
using YGZ.Discount.Application.Coupons.Commands.DeleteCoupon;
using YGZ.Discount.Application.Coupons.Commands.UpdateCoupon;
using YGZ.Discount.Application.Coupons.Queries.GetAllPromotionCoupons;
using YGZ.Discount.Application.Coupons.Queries.GetByCouponCode;
using YGZ.Discount.Application.Events.Commands.AddEventProductSKUs;
using YGZ.Discount.Application.Events.Commands.CreateEvent;
using YGZ.Discount.Application.PromotionCoupons.Commands.CreatePromotionEvent;
using YGZ.Discount.Application.Promotions.Commands.CreatePromotionCategory;
using YGZ.Discount.Application.Promotions.Commands.CreatePromotionGlobal;
using YGZ.Discount.Application.Promotions.Commands.CreatePromotionProduct;
using YGZ.Discount.Application.Promotions.Queries.GetPromotionEventQuery;
using YGZ.Discount.Application.Promotions.Queries.GetPromotionItem;
using YGZ.Discount.Application.Promotions.Queries.GetPromotionItemById;
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

        if (result.IsFailure)
        {
            return new CouponResponse();
        }

        var response = _mapper.Map<CouponResponse>(result.Response!);

        return response;
    }

    public override async Task<GetAllDiscountsResponse> GetAllPromotionCoupons(GetAllPromotionCouponsRequest request, ServerCallContext context)
    {
        var query = new GetAllPromotionCouponsQuery();

        var result = await _sender.Send(query);

        if (result.IsFailure)
        {
            return new GetAllDiscountsResponse();
        }

        var response = new GetAllDiscountsResponse();


        response.PromotionCoupons.AddRange(result.Response!.Select(p => new PromotionCouponModel()
        {
            PromotionCouponId = p.PromotionCouponId,
            PromotionCouponTitle = p.PromotionCouponTitle,
            PromotionCouponCode = p.PromotionCouponCode,
            PromotionCouponDescription = p.PromotionCouponDescription,
            PromotionCouponProductNameTag = (ProductNameTagEnum)ProductNameTag.FromName(p.PromotionCouponProductNameTag, false).Value,
            PromotionCouponPromotionEventType = (PromotionEventTypeEnum)PromotionEventType.FromName(p.PromotionCouponPromotionEventType, false).Value,
            PromotionCouponDiscountState = (DiscountStateEnum)DiscountState.FromName(p.PromotionCouponDiscountState, false).Value,
            PromotionCouponDiscountType = (DiscountTypeEnum)DiscountType.FromName(p.PromotionCouponDiscountType, false).Value,
            PromotionCouponDiscountValue = (double)p.PromotionCouponDiscountValue,
            PromotionCouponMaxDiscountAmount = p.PromotionCouponMaxDiscountAmount.HasValue ? (double)p.PromotionCouponMaxDiscountAmount.Value : 0,
            PromotionCouponValidFrom = p.PromotionCouponValidFrom.HasValue ? p.PromotionCouponValidFrom.Value.ToTimestamp() : null,
            PromotionCouponValidTo = p.PromotionCouponValidTo.HasValue ? p.PromotionCouponValidTo.Value.ToTimestamp() : null,
            PromotionCouponAvailableQuantity = p.PromotionCouponAvailableQuantity
        }));

        return response;
    }

    public override async Task<PromotionEventResponse> GetPromotionEvent(GetPromotionEventRequest request, ServerCallContext context)
    {
        var query = new GetPromotionEventQuery();

        var result = await _sender.Send(query);

        var response = new PromotionEventResponse();

        result.Response!.ForEach(e =>
        {
            var promotionEventResponse = new ListPromtionEventResponse();

            promotionEventResponse.PromotionEvent = new PromotionEventModel()
            {
                PromotionEventId = e.promotionEvent.PromotionEventId,
                PromotionEventTitle = e.promotionEvent.PromotionEventTitle,
                PromotionEventDescription = e.promotionEvent.PromotionEventDescription,
                PromotionEventPromotionEventType = (PromotionEventTypeEnum)PromotionEventType.FromName(e.promotionEvent.PromotionEventType, false).Value,
                PromotionEventState = (DiscountStateEnum)DiscountState.FromName(e.promotionEvent.PromotionEventState, false).Value,
                PromotionEventValidFrom = e.promotionEvent.PromotionEventValidFrom.HasValue ? e.promotionEvent.PromotionEventValidFrom.Value.ToTimestamp() : null,
                PromotionEventValidTo = e.promotionEvent.PromotionEventValidTo.HasValue ? e.promotionEvent.PromotionEventValidTo.Value.ToTimestamp() : null
            };

            if (e.PromotionProducts is not null)
            {
                promotionEventResponse.PromotionProducts.AddRange(e.PromotionProducts.Select(p => _mapper.Map<PromotionProductModel>(p)));
            }

            if (e.PromotionCategories is not null)
            {
                promotionEventResponse.PromotionCategories.AddRange(e.PromotionCategories.Select(c => _mapper.Map<PromotionCategoryModel>(c)));
            }

            response.PromotionEvents.Add(promotionEventResponse);
        });

        return response;
    }

    public override async Task<PromotionItemModel> GetPromotionItemById(GetPromotionItemByIdRequest request, ServerCallContext context)
    {
        var query = new GetPromotionItemByIdQuery(request.PromotionId);

        var result = await _sender.Send(query);

        var response = _mapper.Map<PromotionItemModel>(result.Response!);

        return response;
    }

    public override async Task<PromotionItemsRepsonse> GetPromotionItems(GetPromotionItemsRequest request, ServerCallContext context)
    {
        var query = new GetPromotionItemsQuery();

        var result = await _sender.Send(query);

        if (result.IsFailure)
        {
            return new PromotionItemsRepsonse();
        }

        var response = new PromotionItemsRepsonse();

        response.PromotionItems.AddRange(result.Response!.Select(p => new PromotionItemModel()
        {
            PromotionItemId = p.PromotionItemId,
            PromotionItemProductId = p.ProductId,
            PromotionItemTitle = p.Title,
            PromotionItemDescription = p.Description,
            PromotionItemNameTag = (ProductNameTagEnum)ProductNameTag.FromName(p.ProductNameTag, false).Value,
            PromotionItemPromotionEventType = (PromotionEventTypeEnum)PromotionEventType.FromName(p.PromotionEventType, false).Value,
            PromotionItemDiscountState = (DiscountStateEnum)DiscountState.FromName(p.DiscountState, false).Value,
            PromotionItemDiscountType = (DiscountTypeEnum)DiscountType.FromName(p.DiscountType, false).Value,
            PromotionItemEndDiscountType = (EndDiscountEnum)EndDiscountType.FromName(p.EndDiscountType, false).Value,
            PromotionItemDiscountValue = (double)p.DiscountValue,
            PromotionItemValidFrom = p.ValidFrom.HasValue ? p.ValidFrom.Value.ToTimestamp() : null,
            PromotionItemValidTo = p.ValidTo.HasValue ? p.ValidTo.Value.ToTimestamp() : null,
            PromotionItemAvailableQuantity = p.AvailableQuantity,
            PromotionItemProductImage = p.PromotionItemProductImage,
            PromotionItemProductSlug = p.PromotionItemProductSlug
        }));

        return response;
    }

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

    public override async Task<BooleanResponse> UpdateDiscountCoupon(UpdateDiscountCouponRequest request, ServerCallContext context)
    {
        var cmd = _mapper.Map<UpdateCouponCommand>(request);

        var result = await _sender.Send(cmd);

        var response = _mapper.Map<BooleanResponse>(result.Response);

        return response;
    }

    public override async Task<BooleanResponse> DeleteDiscountCoupon(DeleteDiscountCouponRequest request, ServerCallContext context)
    {
        var cmd = _mapper.Map<DeleteCouponCommand>(request);

        var result = await _sender.Send(cmd);

        var response = _mapper.Map<BooleanResponse>(result.Response);

        return response;
    }

    public override async Task<BooleanResponse> CreateEventGrpc(CreateEventRequest request, ServerCallContext context)
    {
        var cmd = _mapper.Map<CreateEventCommand>(request);

        var result = await _sender.Send(cmd);

        var response = _mapper.Map<BooleanResponse>(result.Response);

        return response;
    }

    public override async Task<BooleanResponse> AddEventProductSKUsGrpc(AddEventProductSKUsRequest request, ServerCallContext context)
    {
        var cmd = _mapper.Map<AddEventProductSKUsCommand>(request);

        var result = await _sender.Send(cmd);

        var response = _mapper.Map<BooleanResponse>(result.Response);

        return response;
    }
}
