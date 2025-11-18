using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Errors;
using YGZ.Discount.Grpc.Protos;

namespace YGZ.Catalog.Application.Promotions.Events.CreateEvent;

public class CreateEventHandler : ICommandHandler<CreateEventCommand, bool>
{
    private readonly ILogger<CreateEventHandler> _logger;
    private readonly DiscountProtoService.DiscountProtoServiceClient _discountProtoServiceClient;

    public CreateEventHandler(ILogger<CreateEventHandler> logger, DiscountProtoService.DiscountProtoServiceClient discountProtoServiceClient)
    {
        _logger = logger;
        _discountProtoServiceClient = discountProtoServiceClient;
    }

    public async Task<Result<bool>> Handle(CreateEventCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var grpcRequest = new CreateEventRequest
            {
                Title = request.Title,
                Description = request.Description,
                StartDate = Timestamp.FromDateTime(DateTime.SpecifyKind(request.StartDate, DateTimeKind.Utc)),
                EndDate = Timestamp.FromDateTime(DateTime.SpecifyKind(request.EndDate, DateTimeKind.Utc))
            };

            var grpcResponse = await _discountProtoServiceClient.CreateEventGrpcAsync(grpcRequest, cancellationToken: cancellationToken);

            return grpcResponse.IsSuccess;
        }
        catch (RpcException rpcEx)
        {
            _logger.LogError(rpcEx, "Failed to create event in Discount service.");
            return Result<bool>.Failure(Error.GrpcError(rpcEx.StatusCode.ToString(), rpcEx.Status.Detail));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while creating event.");
            return false;
        }
    }
}
