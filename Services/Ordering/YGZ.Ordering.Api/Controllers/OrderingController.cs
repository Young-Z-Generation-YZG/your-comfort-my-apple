using Keycloak.AuthServices.Authorization;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using YGZ.BuildingBlocks.Shared.Extensions;
using YGZ.Ordering.Api.Contracts;
using YGZ.Ordering.Application.Orders.Commands.UpdateOrderStatus;
using YGZ.Ordering.Application.Orders.Queries.GetOrderByUser;
using YGZ.Ordering.Application.Orders.Queries.GetOrderItemsByOrderId;
using YGZ.Ordering.Application.Orders.Queries.GetOrders;
using static YGZ.BuildingBlocks.Shared.Constants.AuthorizationConstants;

namespace YGZ.Ordering.Api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/orders")]
[OpenApiTag("orders", Description = "Manage orders.")]
[Authorize(Policy = Policies.REQUIRE_AUTHENTICATION)]
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

    [HttpGet("admin")]
    [Authorize(Policy = Policies.R__ADMIN_SUPER___RS__ALL)]
    [ProtectedResource(Resources.RESOURCE_ORDERS, Scopes.ALL)]
    public async Task<IActionResult> GetAllOrders([FromQuery] GetOrdersRequest request, CancellationToken cancellationToken)
    {
        var query = _mapper.Map<GetOrdersByAdminQuery>(request);

        var result = await _sender.Send(query, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }

    [HttpGet()]
    public async Task<IActionResult> GetUserOrders([FromQuery] GetOrdersPaginationRequest request, CancellationToken cancellationToken)
    {
        var query = _mapper.Map<GetOrdersByUserQuery>(request);

        var result = await _sender.Send(query, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }

    [HttpGet("{orderId}")]
    public async Task<IActionResult> GetOrderDetails([FromRoute] string orderId, CancellationToken cancellationToken)
    {
        var query = new GetOrderDetailsByIdQuery(orderId);

        var result = await _sender.Send(query, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }

    // [HttpPatch("{orderId}/status/confirm")]
    // public async Task<IActionResult> ConfirmOrder([FromRoute] string orderId, CancellationToken cancellationToken)
    // {
    //     var cmd = new ConfirmOrderCommand(orderId);

    //     var result = await _sender.Send(cmd, cancellationToken);

    //     return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    // }

    // [HttpPatch("{orderId}/status/cancel")]
    // public async Task<IActionResult> CancelOrder([FromRoute] string orderId, CancellationToken cancellationToken)
    // {
    //     var cmd = new CancelOrderCommand(orderId);

    //     var result = await _sender.Send(cmd, cancellationToken);

    //     return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    // }

    [HttpPatch("online/{OrderId}/status")]
    public async Task<IActionResult> UpdateOrderStatus([FromRoute] string OrderId, [FromBody] UpdateOrderStatusRequest request, CancellationToken cancellationToken)
    {
        var cmd = new UpdateOrderStatusCommand
        {
            OrderId = OrderId,
            UpdateStatus = request.UpdateStatus
        };

        var result = await _sender.Send(cmd, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }


    //[HttpPatch("/{orderId}/status/refund")]
}
