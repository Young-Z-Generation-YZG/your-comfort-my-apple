using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using YGZ.Basket.Application.Baskets.Queries.GetBasket;
using YGZ.Basket.Api.Common.Extensions;
using YGZ.Basket.Application.Contracts;
using YGZ.Basket.Application.Baskets.Commands.StoreBasket;
using YGZ.Basket.Application.Baskets.Commands.DeleteBasket;

namespace YGZ.Basket.Api.Controllers;

[Route("api/v{version:apiVersion}/baskets")]
[ApiVersion(1)]
public class BasketController : ApiController
{
    private readonly ISender _mediator;
    private readonly IMapper _mapper;

    public BasketController(ISender mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetBasketByUserId(string userId, CancellationToken cancellationToken = default)
    {
        var query = new GetBasketByUserIdQuery(userId);

        var result = await _mediator.Send(query, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }

    [HttpPost]
    public async Task<IActionResult> StoreBasket([FromBody] StoreBasketRequest request, CancellationToken cancellationToken = default)
    {
        var cmd = _mapper.Map<StoreBasketCommand>(request);

        var result = await _mediator.Send(cmd, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }

    [HttpDelete("{userId}")]
    public async Task<IActionResult> DeleteBasketByUserId(string userId, CancellationToken cancellationToken = default)
    {
        var cmd = new DeleteBasketCommand(userId);

        var result = await _mediator.Send(cmd, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }
}