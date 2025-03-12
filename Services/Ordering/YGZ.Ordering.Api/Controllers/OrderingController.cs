using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using YGZ.Ordering.Api.Contracts;
using YGZ.Ordering.Application.Orders.Commands;
using YGZ.BuildingBlocks.Shared.Extensions;

namespace YGZ.Ordering.Api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/ordering")]
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

    [HttpPost()]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request, CancellationToken cancellationToken)
    {
        var cmd = _mapper.Map<CreateOrderCommand>(request);

        var result = await _sender.Send(cmd, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }
}
