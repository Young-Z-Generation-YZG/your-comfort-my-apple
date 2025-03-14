using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using YGZ.Basket.Api.Contracts;
using YGZ.Basket.Application.ShoppingCarts.Commands.CheckoutBasket;
using YGZ.Basket.Application.ShoppingCarts.Commands.DeleteBasket;
using YGZ.Basket.Application.ShoppingCarts.Commands.StoreBasket;
using YGZ.Basket.Application.ShoppingCarts.Queries.GetBasket;
using YGZ.BuildingBlocks.Shared.Extensions;

namespace YGZ.Basket.Api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/baskets")]
[OpenApiTag("baskets", Description = "Manage baskets.")]
//[ProtectedResource("baskets")]
[AllowAnonymous]
public class BasketController : ApiController
{
    private readonly ILogger<BasketController> _logger;
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public BasketController(ILogger<BasketController> logger, ISender sender, IMapper mapper)
    {
        _logger = logger;
        _sender = sender;
        _mapper = mapper;
    }

    [HttpPost()]
    public async Task<IActionResult> StoreBasket([FromBody] StoreBasketRequest request, CancellationToken cancellationToken)
    {
        var cmd = _mapper.Map<StoreBasketCommand>(request);

        var result = await _sender.Send(cmd, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }

    [HttpPost("checkout")]
    public async Task<IActionResult> CheckoutBasket([FromBody] CheckoutBasketRequest request, CancellationToken cancellationToken)
    {
        var cmd = _mapper.Map<CheckoutBasketCommand>(request);

        var result = await _sender.Send(cmd, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }

    [HttpGet()]
    public async Task<IActionResult> GetBasket(CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetBasketQuery(), cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }

    [HttpDelete()]
    public async Task<IActionResult> DeleteBasket(CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new DeleteBasketCommand(), cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }
}
