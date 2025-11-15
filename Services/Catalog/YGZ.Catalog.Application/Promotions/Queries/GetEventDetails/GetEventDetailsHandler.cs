using Grpc.Core;
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Errors;
using YGZ.Discount.Grpc.Protos;
using SharedEventItemResponse = YGZ.BuildingBlocks.Shared.Contracts.Discounts.EventItemResponse;
using SharedEventResponse = YGZ.BuildingBlocks.Shared.Contracts.Discounts.EventResponse;

namespace YGZ.Catalog.Application.Promotions.Queries.GetEventDetails;

public class GetEventDetailsHandler : IQueryHandler<GetEventDetailsQuery, SharedEventResponse>
{
    private readonly ILogger<GetEventDetailsHandler> _logger;
    private readonly DiscountProtoService.DiscountProtoServiceClient _discountProtoServiceClient;

    public GetEventDetailsHandler(ILogger<GetEventDetailsHandler> logger, DiscountProtoService.DiscountProtoServiceClient discountProtoServiceClient)
    {
        _logger = logger;
        _discountProtoServiceClient = discountProtoServiceClient;
    }

    public async Task<Result<SharedEventResponse>> Handle(GetEventDetailsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var grpcResponse = await _discountProtoServiceClient.GetEventDetailsGrpcAsync(
                new GetEventDetailsGrpcRequest { EventId = request.EventId },
                cancellationToken: cancellationToken);

            return MapToSharedEventResponse(grpcResponse);
        }
        catch (RpcException rpcEx)
        {
            _logger.LogError(rpcEx, "Failed to retrieve event {EventId} from Discount service.", request.EventId);
            return Result<SharedEventResponse>.Failure(Error.GrpcError(rpcEx.StatusCode.ToString(), rpcEx.Status.Detail));
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
            ModelName = grpcItem.Model?.Name ?? string.Empty,
            NormalizedModel = grpcItem.Model?.NormalizedName ?? string.Empty,
            ColorName = grpcItem.Color?.Name ?? string.Empty,
            NormalizedColor = grpcItem.Color?.NormalizedName ?? string.Empty,
            ColorHexCode = grpcItem.Color?.HexCode ?? string.Empty,
            StorageName = grpcItem.Storage?.Name ?? string.Empty,
            NormalizedStorage = grpcItem.Storage?.NormalizedName ?? string.Empty,
            ProductClassification = grpcItem.ProductClassification.ToString(),
            ImageUrl = grpcItem.DisplayImageUrl,
            DiscountType = grpcItem.DiscountType.ToString(),
            DiscountValue = (decimal)(grpcItem.DiscountValue ?? 0),
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
}
