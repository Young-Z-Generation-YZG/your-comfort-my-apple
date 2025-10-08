
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MapsterMapper;
using MediatR;
using YGZ.Discount.Application.Events.Commands.AddEventItem;
using YGZ.Discount.Application.Events.Commands.CreateEvent;
using YGZ.Discount.Application.Events.Queries.GetEventWithEventItems;
using YGZ.Discount.Grpc.Protos;
using EDiscountType = YGZ.Discount.Grpc.Protos.EDiscountType;
using EProductType = YGZ.Discount.Grpc.Protos.EProductType;

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

    //public override async Task<CouponResponse> GetDiscountByCode(GetDiscountRequest request, ServerCallContext context)
    //{
    //    var query = new GetCouponByCodeQuery(request.Code);

    //    var result = await _sender.Send(query);

    //    if (result.IsFailure)
    //    {
    //        return new CouponResponse();
    //    }

    //    var response = _mapper.Map<CouponResponse>(result.Response!);

    //    return response;
    //}

    //public override async Task<GetAllDiscountsResponse> GetAllPromotionCoupons(GetAllPromotionCouponsRequest request, ServerCallContext context)
    //{
    //    var query = new GetAllPromotionCouponsQuery();

    //    var result = await _sender.Send(query);

    //    if (result.IsFailure)
    //    {
    //        return new GetAllDiscountsResponse();
    //    }

    //    var response = new GetAllDiscountsResponse();


    //    response.PromotionCoupons.AddRange(result.Response!.Select(p => new PromotionCouponModel()
    //    {
    //        PromotionCouponId = p.PromotionCouponId,
    //        PromotionCouponTitle = p.PromotionCouponTitle,
    //        PromotionCouponCode = p.PromotionCouponCode,
    //        PromotionCouponDescription = p.PromotionCouponDescription,
    //        PromotionCouponProductNameTag = (ProductNameTagEnum)ProductNameTag.FromName(p.PromotionCouponProductNameTag, false).Value,
    //        PromotionCouponPromotionEventType = (PromotionEventTypeEnum)PromotionEventType.FromName(p.PromotionCouponPromotionEventType, false).Value,
    //        PromotionCouponDiscountState = (DiscountStateEnum)DiscountState.FromName(p.PromotionCouponDiscountState, false).Value,
    //        PromotionCouponDiscountType = (DiscountTypeEnum)DiscountType.FromName(p.PromotionCouponDiscountType, false).Value,
    //        PromotionCouponDiscountValue = (double)p.PromotionCouponDiscountValue,
    //        PromotionCouponMaxDiscountAmount = p.PromotionCouponMaxDiscountAmount.HasValue ? (double)p.PromotionCouponMaxDiscountAmount.Value : 0,
    //        PromotionCouponValidFrom = p.PromotionCouponValidFrom.HasValue ? p.PromotionCouponValidFrom.Value.ToTimestamp() : null,
    //        PromotionCouponValidTo = p.PromotionCouponValidTo.HasValue ? p.PromotionCouponValidTo.Value.ToTimestamp() : null,
    //        PromotionCouponAvailableQuantity = p.PromotionCouponAvailableQuantity
    //    }));

    //    return response;
    //}



    //public override async Task<BooleanResponse> CreatePromotionCoupon(CreatePromotionCouponModel request, ServerCallContext context)
    //{
    //    var cmd = _mapper.Map<CreatePromotionCouponCommand>(request);

    //    var result = await _sender.Send(cmd);

    //    var response = _mapper.Map<BooleanResponse>(result.Response);

    //    return response;
    //}

    //public override async Task<BooleanResponse> CreatePromotionItem(CreatePromotionItemModelRequest request, ServerCallContext context)
    //{
    //    var cmd = _mapper.Map<CreatePromotionItemCommand>(request);

    //    var result = await _sender.Send(cmd);

    //    var response = _mapper.Map<BooleanResponse>(result.Response);

    //    return response;
    //}


    //public override async Task<BooleanResponse> DeleteDiscountCoupon(DeleteDiscountCouponRequest request, ServerCallContext context)
    //{
    //    var cmd = _mapper.Map<DeleteCouponCommand>(request);

    //    var result = await _sender.Send(cmd);

    //    var response = _mapper.Map<BooleanResponse>(result.Response);

    //    return response;
    //}

    // OTHERS METHOD
    public override async Task<BooleanResponse> CreateEventGrpc(CreateEventRequest request, ServerCallContext context)
    {
        var cmd = _mapper.Map<CreateEventCommand>(request);

        var result = await _sender.Send(cmd);

        var response = _mapper.Map<BooleanResponse>(result.Response);

        return response;
    }

    public override async Task<BooleanResponse> AddEventItemsGrpc(AddEventItemsRequest request, ServerCallContext context)
    {
        var cmd = _mapper.Map<AddEventItemsCommand>(request);

        var result = await _sender.Send(cmd);

        var response = _mapper.Map<BooleanResponse>(result.Response);

        return response;
    }

    // GET METHODS
    public override async Task<GetEventWithEventItemsResponse> GetEventWithEventItemsGrpc(GetEventWithEventItemsRequest request, ServerCallContext context)
    {
        var query = new GetEventWithEventItemsQuery();

        var result = await _sender.Send(query);

        if (result.IsFailure)
        {
            return new GetEventWithEventItemsResponse();
        }

        var eventData = result.Response!;

        // Map Event
        var eventModel = new EventModel
        {
            Id = eventData.Event.Id,
            Title = eventData.Event.Title,
            Description = eventData.Event.Description,
            StartDate = eventData.Event.StartDate.HasValue
                ? eventData.Event.StartDate.Value.ToTimestamp()
                : null,
            EndDate = eventData.Event.EndDate.HasValue
                ? eventData.Event.EndDate.Value.ToTimestamp()
                : null
        };

        // Map EventItems
        var eventItems = eventData.EventItems.Select(ei => new EventItemModel
        {
            Id = ei.Id?.ToString() ?? string.Empty,
            Model = new ModelValueObject
            {
                Name = ei.ModelName ?? string.Empty,
                NormalizedName = ei.NormalizedModel ?? string.Empty
            },
            Color = new ColorValueObject
            {
                Name = ei.ColorName ?? string.Empty,
                NormalizedName = ei.NormalizedColor ?? string.Empty,
                HexCode = ei.ColorHexCode ?? string.Empty
            },
            Storage = new StorageValueObject
            {
                Name = ei.StorageName ?? string.Empty,
                NormalizedName = ei.NormalizedStorage ?? string.Empty
            },
            DisplayImageUrl = ei.ImageUrl ?? string.Empty,
            ProductType = ConvertToProductTypeEnum(ei.ProductType),
            DiscountType = ConvertToDiscountTypeEnum(ei.DiscountType),
            DiscountValue = (double)ei.DiscountValue,
            OriginalPrice = (double)ei.OriginalPrice,
            Stock = ei.Stock,
            Sold = ei.Sold
        }).ToList();

        var response = new GetEventWithEventItemsResponse
        {
            Event = eventModel
        };

        response.EventItems.AddRange(eventItems);

        return response;
    }

    private static EProductType ConvertToProductTypeEnum(string productType)
    {
        return productType.ToUpper() switch
        {
            "IPHONE" => EProductType.ProductTypeIphone,
            "IPAD" => EProductType.ProductTypeIpad,
            "MACBOOK" => EProductType.ProductTypeMacbook,
            "WATCH" => EProductType.ProductTypeWatch,
            "HEADPHONE" => EProductType.ProductTypeHeadphone,
            "ACCESSORY" => EProductType.ProductTypeAccessory,
            _ => EProductType.ProductTypeUnknown
        };
    }

    private static EDiscountType ConvertToDiscountTypeEnum(string discountType)
    {
        return discountType.ToUpper() switch
        {
            "PERCENTAGE" => EDiscountType.DiscountTypePercentage,
            "FIXED" => EDiscountType.DiscountTypeFixed,
            _ => EDiscountType.DiscountTypeUnknown
        };
    }
}
