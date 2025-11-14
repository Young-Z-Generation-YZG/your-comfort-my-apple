using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MapsterMapper;
using MediatR;
using YGZ.BuildingBlocks.Shared.Errors;
using YGZ.Discount.Application.Coupons.Commands.CreateCoupon;
using YGZ.Discount.Application.Coupons.Commands.UpdateCouponQuantity;
using YGZ.Discount.Application.Coupons.Queries.GetAllPromotionCoupons;
using YGZ.Discount.Application.Coupons.Queries.GetByCouponCode;
using YGZ.Discount.Application.EventItem.Queries.GetEventItemById;
using YGZ.Discount.Application.Events.Commands.AddEventItem;
using YGZ.Discount.Application.Events.Commands.CreateEvent;
using YGZ.Discount.Application.Events.Queries.GetEventDetails;
using YGZ.Discount.Application.Events.Queries.GetEvents;
using YGZ.Discount.Grpc.Protos;
using DiscountContracts = YGZ.BuildingBlocks.Shared.Contracts.Discounts;

namespace YGZ.Discount.Grpc.GrpcServices;

public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
{
    private readonly IMapper _mapper;
    private readonly ISender _sender;
    private readonly ILogger<DiscountService> _logger;

    public DiscountService(IMapper mapper,
                           ISender sender,
                           ILogger<DiscountService> logger)
    {
        _mapper = mapper;
        _sender = sender;
        _logger = logger;
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
            UserId = result.Response.UserId,
            Code = result.Response.Code,
            Title = result.Response.Title,
            Description = result.Response.Description,
            CategoryType = ConvertToEProductClassificationGrpc(result.Response.ProductClassification),
            DiscountType = ConvertToEDiscountTypeGrpc(result.Response.DiscountType),
            DiscountValue = (double)result.Response.DiscountValue,
            MaxDiscountAmount = result.Response.MaxDiscountAmount != null ? (double)result.Response.MaxDiscountAmount : 0,
            AvailableQuantity = result.Response.AvailableQuantity,
            Stock = result.Response.Stock
        };
    }

    public override async Task<GetCouponsResponse> GetCouponsGrpc(GetCouponsRequest request, ServerCallContext context)
    {
        var query = new GetCouponsQuery();

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

        var response = new GetCouponsResponse();

        // Map each CouponResponse to CouponModel
        var couponModels = result.Response!.Select(c => new CouponModel
        {
            Id = c.Id,
            UserId = c.UserId ?? string.Empty,
            Code = c.Code,
            Title = c.Title,
            Description = c.Description,
            CategoryType = ConvertToEProductClassificationGrpc(c.ProductClassification),
            DiscountType = ConvertToEDiscountTypeGrpc(c.DiscountType),
            DiscountValue = (double)c.DiscountValue,
            MaxDiscountAmount = c.MaxDiscountAmount.HasValue ? (double)c.MaxDiscountAmount.Value : 0,
            AvailableQuantity = c.AvailableQuantity,
            Stock = c.Stock
        });

        response.Coupons.AddRange(couponModels);

        return response;
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
            ProductClassification = ConvertToEProductClassificationGrpc(ei.ProductClassification),
            DiscountType = ConvertToEDiscountTypeGrpc(ei.DiscountType),
            DiscountValue = (double)ei.DiscountValue,
            OriginalPrice = (double)ei.OriginalPrice,
            Stock = ei.Stock,
            Sold = ei.Sold
        };

        return eventItemModel;
    }

    public override async Task<ListEventResponse> GetEventsGrpc(GetEventsGrpcRequest request, ServerCallContext context)
    {
        var query = new GetEventsQuery();

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

        var response = new ListEventResponse();

        foreach (var discountEvent in result.Response ?? Enumerable.Empty<DiscountContracts.EventResponse>())
        {
            response.EventResponses.Add(MapEventResponse(discountEvent));
        }

        return response;
    }
    private static EventResponse MapEventResponse(DiscountContracts.EventResponse discountEvent)
    {
        var eventModel = new EventModel
        {
            Id = discountEvent.Id,
            Title = discountEvent.Title,
            Description = discountEvent.Description ?? string.Empty,
            StartDate = ToTimestampUtc(discountEvent.StartDate),
            EndDate = ToTimestampUtc(discountEvent.EndDate),
            CreatedAt = ToTimestampUtc(discountEvent.CreatedAt),
            UpdatedAt = ToTimestampUtc(discountEvent.UpdatedAt),
            UpdatedBy = discountEvent.UpdatedBy ?? string.Empty,
            IsDeleted = discountEvent.IsDeleted,
            DeletedAt = ToTimestampUtc(discountEvent.DeletedAt),
            DeletedBy = discountEvent.DeletedBy ?? string.Empty
        };

        var eventResponse = new EventResponse { EventModel = eventModel };

        if (discountEvent.EventItems is not null && discountEvent.EventItems.Count > 0)
        {
            eventResponse.EventItemModels.AddRange(discountEvent.EventItems.Select(MapEventItemModel));
        }

        return eventResponse;
    }



    public override async Task<EventResponse> GetEventDetailsGrpc(GetEventDetailsGrpcRequest request, ServerCallContext context)
    {
        var query = new GetEventDetailsQuery
        {
            EventId = request.EventId
        };

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

        return MapEventResponse(result.Response!);
    }

    //public override async Task<GetEventWithEventItemsResponse> GetEventWithEventItemsGrpc(GetEventWithEventItemsRequest request, ServerCallContext context)
    //{
    //    var query = new GetEventWithEventItemsQuery();

    //    var result = await _sender.Send(query);

    //    if (result.IsFailure)
    //    {
    //        throw new RpcException(new Status(
    //            MapErrorToStatusCode(result.Error),
    //            result.Error.Message
    //        ), new Metadata
    //        {
    //            { "error-code", result.Error.Code },
    //            { "service-name", "DiscountService" }
    //        });
    //    }

    //    var eventData = result.Response!;

    //    // Map Event
    //    var eventModel = new EventModel
    //    {
    //        Id = eventData.Event.Id,
    //        Title = eventData.Event.Title,
    //        Description = eventData.Event.Description,
    //        StartDate = eventData.Event.StartDate.HasValue
    //            ? eventData.Event.StartDate.Value.ToTimestamp()
    //            : null,
    //        EndDate = eventData.Event.EndDate.HasValue
    //            ? eventData.Event.EndDate.Value.ToTimestamp()
    //            : null
    //    };

    //    // Map EventItems
    //    var eventItems = eventData.EventItems.Select(ei => new EventItemModel
    //    {
    //        Id = ei.Id?.ToString() ?? string.Empty,
    //        Model = new ModelValueObject
    //        {
    //            Name = ei.ModelName ?? string.Empty,
    //            NormalizedName = ei.NormalizedModel ?? string.Empty
    //        },
    //        Color = new ColorValueObject
    //        {
    //            Name = ei.ColorName ?? string.Empty,
    //            NormalizedName = ei.NormalizedColor ?? string.Empty,
    //            HexCode = ei.ColorHexCode ?? string.Empty
    //        },
    //        Storage = new StorageValueObject
    //        {
    //            Name = ei.StorageName ?? string.Empty,
    //            NormalizedName = ei.NormalizedStorage ?? string.Empty
    //        },
    //        DisplayImageUrl = ei.ImageUrl ?? string.Empty,
    //        ProductClassification = ConvertToEProductClassificationGrpc(ei.ProductClassification),
    //        DiscountType = ConvertToEDiscountTypeGrpc(ei.DiscountType),
    //        DiscountValue = (double)ei.DiscountValue,
    //        OriginalPrice = (double)ei.OriginalPrice,
    //        Stock = ei.Stock,
    //        Sold = ei.Sold
    //    }).ToList();

    //    var response = new GetEventWithEventItemsResponse
    //    {
    //        Event = eventModel
    //    };

    //    response.EventItems.AddRange(eventItems);

    //    return response;
    //}



    // privates methods
    private static EProductClassificationGrpc ConvertToEProductClassificationGrpc(string productClassification)
    {
        return productClassification.ToUpper() switch
        {
            "IPHONE" => EProductClassificationGrpc.ProductClassificationIphone,
            "IPAD" => EProductClassificationGrpc.ProductClassificationIpad,
            "MACBOOK" => EProductClassificationGrpc.ProductClassificationMacbook,
            "WATCH" => EProductClassificationGrpc.ProductClassificationWatch,
            "HEADPHONE" => EProductClassificationGrpc.ProductClassificationHeadphone,
            "ACCESSORY" => EProductClassificationGrpc.ProductClassificationAccessory,
            _ => EProductClassificationGrpc.ProductClassificationUnknown
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

    private static EventItemModel MapEventItemModel(DiscountContracts.EventItemResponse item)
    {
        return new EventItemModel
        {
            Id = item.Id,
            EventId = item.EventId,
            SkuId = string.Empty,
            TenantId = string.Empty,
            BranchId = string.Empty,
            Model = new ModelValueObject
            {
                Name = item.ModelName ?? string.Empty,
                NormalizedName = item.NormalizedModel ?? string.Empty
            },
            Color = new ColorValueObject
            {
                Name = item.ColorName ?? string.Empty,
                NormalizedName = item.NormalizedColor ?? string.Empty,
                HexCode = item.ColorHexCode ?? string.Empty
            },
            Storage = new StorageValueObject
            {
                Name = item.StorageName ?? string.Empty,
                NormalizedName = item.NormalizedStorage ?? string.Empty
            },
            DisplayImageUrl = item.ImageUrl ?? string.Empty,
            ProductClassification = ConvertToEProductClassificationGrpc(item.ProductClassification),
            DiscountType = ConvertToEDiscountTypeGrpc(item.DiscountType),
            DiscountValue = (double)item.DiscountValue,
            OriginalPrice = (double)item.OriginalPrice,
            Stock = item.Stock,
            Sold = item.Sold,
            CreatedAt = ToTimestampUtc(item.CreatedAt),
            UpdatedAt = ToTimestampUtc(item.UpdatedAt),
            UpdatedBy = item.UpdatedBy ?? string.Empty,
            IsDeleted = item.IsDeleted,
            DeletedAt = ToTimestampUtc(item.DeletedAt),
            DeletedBy = item.DeletedBy ?? string.Empty
        };
    }

    private static Timestamp ToTimestampUtc(DateTime dateTime)
    {
        return Timestamp.FromDateTime(DateTime.SpecifyKind(dateTime, DateTimeKind.Utc));
    }

    private static Timestamp? ToTimestampUtc(DateTime? dateTime)
    {
        return dateTime is null ? null : ToTimestampUtc(dateTime.Value);
    }

    private static StatusCode MapErrorToStatusCode(Error error)
    {
        return error.Code switch
        {
            // Not Found errors
            "Coupon.NotFound" => StatusCode.NotFound,
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
