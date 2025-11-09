using Grpc.Core;
using MapsterMapper;
using MediatR;
using YGZ.Ordering.Api.Protos;
using YGZ.Ordering.Application.OrderItems.Commands.UpdateIsReviewed;

namespace YGZ.Ordering.Api.RpcServices;

public class OrderingRpcService : OrderingProtoService.OrderingProtoServiceBase
{
    private readonly IMapper _mapper;
    private readonly ISender _sender;

    public OrderingRpcService(IMapper mapper, ISender sender)
    {
        _mapper = mapper;
        _sender = sender;
    }

    public override async Task<BooleanGrpcResponse> UpdateOrderItemIsReviewedGrpc(UpdateOrderItemIsReviewedGrpcRequest request, ServerCallContext context)
    {
        var cmd = new UpdateIsReviewedCommand
        {
            OrderItemId = request.OrderItemId,
            IsReviewed = request.IsReviewed
        };

        var result = await _sender.Send(cmd, context.CancellationToken);

        if (result.IsFailure)
        {
            return new BooleanGrpcResponse
            {
                IsSuccess = false,
                IsFailure = true,
                ErrorCode = result.Error.Code,
                ErrorMessage = result.Error.Message
            };
        }

        var response = new BooleanGrpcResponse
        {
            IsSuccess = true,
            IsFailure = false,
            ErrorCode = null,
            ErrorMessage = null
        };

        return response;
    }
}
