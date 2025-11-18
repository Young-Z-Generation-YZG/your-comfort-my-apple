using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Errors;
using YGZ.Discount.Grpc.Protos;

namespace YGZ.Catalog.Application.Promotions.Events.UpdateEvent;

public class UpdateEventHandler : ICommandHandler<UpdateEventCommand, bool>
{
    private readonly ILogger<UpdateEventHandler> _logger;
    private readonly DiscountProtoService.DiscountProtoServiceClient _discountProtoServiceClient;

    public UpdateEventHandler(ILogger<UpdateEventHandler> logger,
                              DiscountProtoService.DiscountProtoServiceClient discountProtoServiceClient)
    {
        _logger = logger;
        _discountProtoServiceClient = discountProtoServiceClient;
    }

    public async Task<Result<bool>> Handle(UpdateEventCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var grpcRequest = new UpdateEventGrpcRequest
            {
                EventId = request.EventId
            };

            if (!string.IsNullOrWhiteSpace(request.Title))
            {
                grpcRequest.Title = request.Title;
            }

            if (!string.IsNullOrWhiteSpace(request.Description))
            {
                grpcRequest.Description = request.Description;
            }

            if (request.StartDate.HasValue)
            {
                grpcRequest.StartDate = Timestamp.FromDateTime(DateTime.SpecifyKind(request.StartDate.Value, DateTimeKind.Utc));
            }

            if (request.EndDate.HasValue)
            {
                grpcRequest.EndDate = Timestamp.FromDateTime(DateTime.SpecifyKind(request.EndDate.Value, DateTimeKind.Utc));
            }

            if (request.AddEventItems != null && request.AddEventItems.Any())
            {
                foreach (var item in request.AddEventItems)
                {
                    var discountItem = new DiscountEventItemRequest
                    {
                        SkuId = item.SkuId,
                        DiscountType = ConvertDiscountTypeToGrpc(item.DiscountType),
                        DiscountValue = (double)item.DiscountValue,
                        Stock = item.Stock
                    };
                    grpcRequest.AddEventItems.Add(discountItem);
                }
            }

            if (request.RemoveEventItemIds != null && request.RemoveEventItemIds.Any())
            {
                foreach (var itemId in request.RemoveEventItemIds)
                {
                    grpcRequest.RemoveEventItemIds.Add(itemId);
                }
            }

            var grpcResponse = await _discountProtoServiceClient.UpdateEventGrpcAsync(grpcRequest, cancellationToken: cancellationToken);

            return grpcResponse.IsSuccess;
        }
        catch (RpcException rpcEx)
        {
            _logger.LogError(rpcEx, "Failed to update event {EventId} in Discount service.", request.EventId);
            return Result<bool>.Failure(Error.GrpcError(rpcEx.StatusCode.ToString(), rpcEx.Status.Detail));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while updating event {EventId}.", request.EventId);
            return false;
        }
    }

    private static EDiscountTypeGrpc ConvertDiscountTypeToGrpc(string discountType)
    {
        return discountType.ToUpper() switch
        {
            "PERCENTAGE" => EDiscountTypeGrpc.DiscountTypePercentage,
            "FIXED" => EDiscountTypeGrpc.DiscountTypeFixed,
            _ => EDiscountTypeGrpc.DiscountTypeUnknown
        };
    }
}
