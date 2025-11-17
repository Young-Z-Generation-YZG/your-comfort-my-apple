using Grpc.Core;
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Errors;
using YGZ.BuildingBlocks.Shared.Enums;
using YGZ.Discount.Grpc.Protos;
using SharedEventItemResponse = YGZ.BuildingBlocks.Shared.Contracts.Discounts.EventItemResponse;
using SharedEventResponse = YGZ.BuildingBlocks.Shared.Contracts.Discounts.EventResponse;

namespace YGZ.Catalog.Application.Promotions.Queries.GetEvents;

public class GetEventsHandler : IQueryHandler<GetEventsQuery, List<SharedEventResponse>>
{
    private readonly ILogger<GetEventsHandler> _logger;
    private readonly DiscountProtoService.DiscountProtoServiceClient _discountProtoServiceClient;

    public GetEventsHandler(ILogger<GetEventsHandler> logger, DiscountProtoService.DiscountProtoServiceClient discountProtoServiceClient)
    {
        _logger = logger;
        _discountProtoServiceClient = discountProtoServiceClient;
    }

    public async Task<Result<List<SharedEventResponse>>> Handle(GetEventsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var grpcResponse = await _discountProtoServiceClient.GetEventsGrpcAsync(new GetEventsGrpcRequest(), cancellationToken: cancellationToken);

            var mappedEvents = grpcResponse.EventResponses
                                           .Select(MapToSharedEventResponse)
                                           .ToList();

            return mappedEvents;
        }
        catch (RpcException rpcEx)
        {
            _logger.LogError(rpcEx, "Failed to retrieve events from Discount service.");
            return Result<List<SharedEventResponse>>.Failure(Error.GrpcError(rpcEx.StatusCode.ToString(), rpcEx.Status.Detail));
        }
    }

    private static SharedEventResponse MapToSharedEventResponse(EventResponse grpcEvent)
    {
        var eventModel = grpcEvent.EventModel;

        return new SharedEventResponse
        {
            Id = eventModel.Id,
            Title = eventModel.Title,
            Description = eventModel.Description,
            StartDate = eventModel.StartDate.ToDateTime(),
            EndDate = eventModel.EndDate.ToDateTime(),
            CreatedAt = eventModel.CreatedAt.ToDateTime(),
            UpdatedAt = eventModel.UpdatedAt.ToDateTime(),
            UpdatedBy = eventModel.UpdatedBy,
            IsDeleted = eventModel.IsDeleted,
            DeletedAt = eventModel.DeletedAt?.ToDateTime(),
            DeletedBy = eventModel.DeletedBy,
            EventItems = grpcEvent.EventItemModels.Select(MapToSharedEventItem).ToList()
        };
    }

    private static SharedEventItemResponse MapToSharedEventItem(EventItemModel grpcItem)
    {
        return new SharedEventItemResponse
        {
            Id = grpcItem.Id,
            EventId = grpcItem.EventId,
            SkuId = grpcItem.SkuId,
            TenantId = grpcItem.TenantId,
            BranchId = grpcItem.BranchId,
            ModelName = grpcItem.Model?.Name ?? string.Empty,
            NormalizedModel = grpcItem.Model?.NormalizedName ?? string.Empty,
            ColorName = grpcItem.Color?.Name ?? string.Empty,
            NormalizedColor = grpcItem.Color?.NormalizedName ?? string.Empty,
            StorageName = grpcItem.Storage?.Name ?? string.Empty,
            NormalizedStorage = grpcItem.Storage?.NormalizedName ?? string.Empty,
            ProductClassification = ConvertProductClassificationGrpcToName(grpcItem.ProductClassification),
            ImageUrl = grpcItem.DisplayImageUrl,
            DiscountType = ConvertDiscountTypeGrpcToName(grpcItem.DiscountType),
            DiscountValue = (decimal)(grpcItem.DiscountValue ?? 0),
            DiscountAmount = (decimal)(grpcItem.DiscountAmount ?? 0),
            FinalPrice = (decimal)(grpcItem.FinalPrice ?? 0),
            OriginalPrice = (decimal)(grpcItem.OriginalPrice ?? 0),
            Stock = grpcItem.Stock ?? 0,
            Sold = grpcItem.Sold ?? 0,
            CreatedAt = grpcItem.CreatedAt.ToDateTime(),
            UpdatedAt = grpcItem.UpdatedAt.ToDateTime(),
            UpdatedBy = grpcItem.UpdatedBy,
            IsDeleted = grpcItem.IsDeleted,
            DeletedAt = grpcItem.DeletedAt?.ToDateTime(),
            DeletedBy = grpcItem.DeletedBy
        };
    }

    private static string ConvertProductClassificationGrpcToName(EProductClassificationGrpc classification)
    {
        return classification switch
        {
            EProductClassificationGrpc.ProductClassificationIphone => EProductClassification.IPHONE.Name,
            EProductClassificationGrpc.ProductClassificationIpad => EProductClassification.IPAD.Name,
            EProductClassificationGrpc.ProductClassificationMacbook => EProductClassification.MACBOOK.Name,
            EProductClassificationGrpc.ProductClassificationWatch => EProductClassification.WATCH.Name,
            EProductClassificationGrpc.ProductClassificationHeadphone => EProductClassification.HEADPHONE.Name,
            EProductClassificationGrpc.ProductClassificationAccessory => EProductClassification.ACCESSORY.Name,
            _ => EProductClassification.UNKNOWN.Name
        };
    }

    private static string ConvertDiscountTypeGrpcToName(EDiscountTypeGrpc discountType)
    {
        return discountType switch
        {
            EDiscountTypeGrpc.DiscountTypePercentage => EDiscountType.PERCENTAGE.Name,
            EDiscountTypeGrpc.DiscountTypeFixed => EDiscountType.FIXED_AMOUNT.Name,
            _ => EDiscountType.UNKNOWN.Name
        };
    }
}
