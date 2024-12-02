using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using MediatR;
using Swashbuckle.AspNetCore.Filters;
using YGZ.Ordering.Application.Orders.Commands.CreateOrder;
using YGZ.Ordering.Api.Common.SwaggerExamples;
using YGZ.Ordering.Api.Common.Extensions;

namespace YGZ.Ordering.Api.Controllers;

[Route("api/v{version:apiVersion}/orders")]
[ApiVersion(1)]
public class OrderController : ApiController
{
    private readonly IMapper _mapper;
    private readonly ISender _mediator;

    public OrderController(IMapper mapper, ISender mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    [HttpPost]
    [SwaggerRequestExample(typeof(CreateOrderCommand), typeof(CreateOrderExample))]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderCommand request, CancellationToken cancellationToken = default)
    {
        //var cmd = _mapper.Map<CreateOrderCommand>(request);

        var result = await _mediator.Send(request, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }
}
