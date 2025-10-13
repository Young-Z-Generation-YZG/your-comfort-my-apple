
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MapsterMapper;
using MediatR;
using YGZ.BuildingBlocks.Shared.Errors;
using YGZ.Discount.Application.Coupons.Commands.CreateCoupon;
using YGZ.Discount.Application.Coupons.Commands.UseCoupon;
using YGZ.Discount.Application.Coupons.Queries.GetByCouponCode;
using YGZ.Discount.Application.EventItem.Queries.GetEventItemById;
using YGZ.Discount.Application.Events.Commands.AddEventItem;
using YGZ.Discount.Application.Events.Commands.CreateEvent;
using YGZ.Discount.Application.Events.Queries.GetEventWithEventItems;
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

    // GET METHODS
    public override async Task<CouponModel> GetCouponByCodeGrpc(GetCouponByCodeRequest request, ServerCallContext context)
    {
        var query = new GetCouponByCodeQuery(request.CouponCode);

        var result = await _sender.Send(query);

        if (result.IsFailure)
        {
            throw new RpcException(new Status(
                MapErrorToStatusCode(result.Error),
                result.Error.Message
            ), new Metadata
            {
                { "error-code", result.Error.Code },
                { "service-name", "DiscountService" }
            });
        }

        return new CouponModel
        {
            Id = result.Response!.Id,
            Code = result.Response.Code,
            Title = result.Response.Title,
            Description = result.Response.Description,
            CategoryType = ConvertToECategoryTypeGrpc(result.Response.CategoryType),
            DiscountType = ConvertToEDiscountTypeGrpc(result.Response.DiscountType),
            DiscountValue = (double)result.Response.DiscountValue,
            MaxDiscountAmount = result.Response.MaxDiscountAmount != null ? (double)result.Response.MaxDiscountAmount : 0,
            AvailableQuantity = result.Response.AvailableQuantity,
            Stock = result.Response.Stock
        };
    }

    public override Task<GetCouponsResponse> GetCouponsGrpc(GetCouponsRequest request, ServerCallContext context)
    {
        return base.GetCouponsGrpc(request, context);
    }


    // POST METHODS
    public override async Task<BooleanResponse> CreateCouponGrpc(CreateCouponRequest request, ServerCallContext context)
    {
        var cmd = _mapper.Map<CreateCouponCommand>(request);

        var result = await _sender.Send(cmd);

        var response = _mapper.Map<BooleanResponse>(result.Response);

        return response;
    }

    public override async Task<BooleanResponse> UseCouponGrpc(UseCouponRequest request, ServerCallContext context)
    {
        var cmd = _mapper.Map<UseCouponCommand>(request);

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

    public override async Task<BooleanResponse> AddEventItemsGrpc(AddEventItemsRequest request, ServerCallContext context)
    {
        var cmd = _mapper.Map<AddEventItemsCommand>(request);

        var result = await _sender.Send(cmd);

        var response = _mapper.Map<BooleanResponse>(result.Response);

        return response;
    }

    // GET METHODS
    public override async Task<EventItemModel> GetEventItemByIdGrpc(GetEventItemByIdRequest request, ServerCallContext context)
    {
        var query = new GetEventItemByIdQuery(request.EventItemId);

        var result = await _sender.Send(query);

        if (result.IsFailure)
        {
            throw new RpcException(new Status(
                MapErrorToStatusCode(result.Error),
                result.Error.Message
            ), new Metadata
            {
                { "error-code", result.Error.Code },
                { "service-name", "DiscountService" }
            });
        }

        var ei = result.Response!;

        var eventItemModel = new EventItemModel
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
            CategoryType = ConvertToECategoryTypeGrpc(ei.CategogyType),
            DiscountType = ConvertToEDiscountTypeGrpc(ei.DiscountType),
            DiscountValue = (double)ei.DiscountValue,
            OriginalPrice = (double)ei.OriginalPrice,
            Stock = ei.Stock,
            Sold = ei.Sold
        };

        return eventItemModel;
    }

    public override async Task<GetEventWithEventItemsResponse> GetEventWithEventItemsGrpc(GetEventWithEventItemsRequest request, ServerCallContext context)
    {
        var query = new GetEventWithEventItemsQuery();

        var result = await _sender.Send(query);

        if (result.IsFailure)
        {
            throw new RpcException(new Status(
                MapErrorToStatusCode(result.Error),
                result.Error.Message
            ), new Metadata
            {
                { "error-code", result.Error.Code },
                { "service-name", "DiscountService" }
            });
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
            CategoryType = ConvertToECategoryTypeGrpc(ei.CategogyType),
            DiscountType = ConvertToEDiscountTypeGrpc(ei.DiscountType),
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



    // privates methods
    private static ECategoryTypeGrpc ConvertToECategoryTypeGrpc(string productType)
    {
        return productType.ToUpper() switch
        {
            "IPHONE" => ECategoryTypeGrpc.CategoryTypeIphone,
            "IPAD" => ECategoryTypeGrpc.CategoryTypeIpad,
            "MACBOOK" => ECategoryTypeGrpc.CategoryTypeMacbook,
            "WATCH" => ECategoryTypeGrpc.CategoryTypeWatch,
            "HEADPHONE" => ECategoryTypeGrpc.CategoryTypeHeadphone,
            "ACCESSORY" => ECategoryTypeGrpc.CategoryTypeAccessory,
            _ => ECategoryTypeGrpc.CategoryTypeUnknown
        };
    }

    private static EDiscountTypeGrpc ConvertToEDiscountTypeGrpc(string discountType)
    {
        return discountType.ToUpper() switch
        {
            "PERCENTAGE" => EDiscountTypeGrpc.DiscountTypePercentage,
            "FIXED" => EDiscountTypeGrpc.DiscountTypeFixed,
            _ => EDiscountTypeGrpc.DiscountTypeUnknown
        };
    }

    /// <summary>
    /// Maps application domain errors to appropriate gRPC status codes
    /// </summary>
    private static StatusCode MapErrorToStatusCode(Error error)
    {
        return error.Code switch
        {
            // Not Found errors
            "Discount.CouponNotFound" => StatusCode.NotFound,
            "Discount.EventNotFound" => StatusCode.NotFound,
            "Discount.EventItemNotFound" => StatusCode.NotFound,

            // Validation/Business Logic errors
            "Discount.CouponExpired" => StatusCode.FailedPrecondition,
            "Discount.CouponInactive" => StatusCode.FailedPrecondition,
            "Discount.InsufficientStock" => StatusCode.FailedPrecondition,
            "Discount.InvalidCouponCode" => StatusCode.InvalidArgument,

            // Default to Internal for unknown errors
            _ => StatusCode.Internal
        };
    }
}
