using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using YGZ.Ordering.Api.Contracts;
using YGZ.BuildingBlocks.Shared.Extensions;
using YGZ.Ordering.Application.Orders.Queries.GetOrders;
using YGZ.Ordering.Application.Orders.Queries.GetOrderByUser;
using YGZ.Ordering.Application.Orders.Queries.GetOrderItemsByOrderId;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using YGZ.Ordering.Application.Orders.Commands.ConfirmOrder;
using YGZ.Ordering.Application.Orders.Commands.UpdateOrderStatus;
using YGZ.Ordering.Application.Orders.Commands.CancelOrder;

namespace YGZ.Ordering.Api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/orders")]
[OpenApiTag("orders", Description = "Manage orders.")]
//[ProtectedResource("orders")]
[AllowAnonymous]
public class OrderingController : ApiController
{
    private readonly ILogger<OrderingController> _logger;
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public OrderingController(ILogger<OrderingController> logger, ISender sender, IMapper mapper)
    {
        _logger = logger;
        _sender = sender;
        _mapper = mapper;
    }

    [HttpGet()]
    public async Task<IActionResult> GetOrders([FromQuery] GetOrdersPaginationRequest request, CancellationToken cancellationToken)
    {
        var query = _mapper.Map<GetOrdersQuery>(request);

        var result = await _sender.Send(query, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetUserOrders([FromQuery] GetOrdersPaginationRequest request, CancellationToken cancellationToken)
    {
        var query = _mapper.Map<GetOrdersByUserQuery>(request);

        var result = await _sender.Send(query, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }

    [HttpGet("{orderId}/order-items")]
    public async Task<IActionResult> GetOrderItems([FromRoute] string orderId, CancellationToken cancellationToken)
    {
        var query = new GetOrderItemsByOrderIdQuery(orderId);

        var result = await _sender.Send(query, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }

    [HttpPatch("/{orderId}/status/confirm")]
    public async Task<IActionResult> ConfirmOrder([FromRoute] string orderId, CancellationToken cancellationToken)
    {
        var cmd = new ConfirmOrderCommand(orderId);

        var result = await _sender.Send(cmd, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }

    [HttpPatch("/{orderId}/status/cancel")]
    public async Task<IActionResult> CancelOrder([FromRoute] string orderId, CancellationToken cancellationToken)
    {
        var cmd = new CancelOrderCommand(orderId);

        var result = await _sender.Send(cmd, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }

    [HttpPatch("/admin/{orderId}/status")]
    public async Task<IActionResult> UpdateOrderStatus([FromRoute] string orderId, [FromQuery] UpdateOrderStatusRequest request, CancellationToken cancellationToken)
    {
        var cmd = _mapper.Map<UpdateOrderStatusCommand>(request);

        var result = await _sender.Send(cmd, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }


    //[HttpPatch("/{orderId}/status/refund")]
}
