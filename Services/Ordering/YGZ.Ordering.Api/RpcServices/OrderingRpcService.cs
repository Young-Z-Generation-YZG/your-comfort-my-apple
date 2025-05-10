using Grpc.Core;
using MapsterMapper;
using MediatR;
using YGZ.Ordering.Api.Protos;
using YGZ.Ordering.Application.OrderItems.Commands;

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

    public override async Task<BooleanResponse> UpdateReviewOrderItem(UpdateReviewOrderItemRquest request, ServerCallContext context)
    {
        var cmd = _mapper.Map<UpdateReviewCommand>(request);

        var result = await _sender.Send(cmd, context.CancellationToken);

        var response = _mapper.Map<BooleanResponse>(result.Response!);

        return response;
    }
}
