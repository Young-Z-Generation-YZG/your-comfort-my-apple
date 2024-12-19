using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using YGZ.Basket.Application.Baskets.Queries.GetBasket;
using YGZ.Basket.Api.Common.Extensions;
using YGZ.Basket.Application.Baskets.Commands.StoreBasket;
using YGZ.Basket.Application.Baskets.Commands.DeleteBasket;
using YGZ.Basket.Application.Baskets.Commands.CheckoutBasket;
using Swashbuckle.AspNetCore.Filters;
using YGZ.BuildingBlocks.Shared.Contracts.Baskets;
using YGZ.Basket.Api.Common.SwaggerExamples;

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
    [SwaggerRequestExample(typeof(StoreBasketRequest), typeof(StoreBasketRequestExample))]
    public async Task<IActionResult> StoreBasket([FromBody] StoreBasketRequest request, [FromQuery] string? coupon = null, CancellationToken cancellationToken = default)
    {
        var cmd = 
            new StoreBasketCommand
            { 
                UserId = request.UserId,
                CouponCode = coupon,
                CartLines = request.Cart_lines.ConvertAll(line => new CartLineCommand(line.ProductItemId,
                                                                                      line.Model,
                                                                                      line.Color,
                                                                                      line.Storage,
                                                                                      line.Price,
                                                                                      line.Quantity)),
            };


        var result = await _mediator.Send(cmd, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }

    [HttpPost("checkout")]
    [SwaggerRequestExample(typeof(CheckoutBasketRequest), typeof(CheckoutBasketRequestExample))]
    public async Task<IActionResult> CheckoutBasket([FromBody] CheckoutBasketRequest request, CancellationToken cancellationToken = default)
    {
        var cmd = _mapper.Map<CheckoutBasketCommand>(request);

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