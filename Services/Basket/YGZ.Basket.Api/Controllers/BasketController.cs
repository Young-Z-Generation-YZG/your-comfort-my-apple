using Keycloak.AuthServices.Authorization;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using YGZ.Basket.Api.Contracts;
using YGZ.Basket.Application.ShoppingCarts.Commands.CheckoutBasket;
using YGZ.Basket.Application.ShoppingCarts.Commands.CheckoutBasketWithBlockchain;
using YGZ.Basket.Application.ShoppingCarts.Commands.DeleteBasket;
using YGZ.Basket.Application.ShoppingCarts.Commands.ProceedCheckout;
using YGZ.Basket.Application.ShoppingCarts.Commands.StoreBasket;
using YGZ.Basket.Application.ShoppingCarts.Commands.StoreBasketItem;
using YGZ.Basket.Application.ShoppingCarts.Commands.StoreEventItem;
using YGZ.Basket.Application.ShoppingCarts.Commands.SyncBasket;
using YGZ.Basket.Application.ShoppingCarts.Queries.GetBasket;
using YGZ.Basket.Application.ShoppingCarts.Queries.GetCheckoutBasket;
using YGZ.BuildingBlocks.Shared.Extensions;
using static YGZ.BuildingBlocks.Shared.Constants.AuthorizationConstants;

namespace YGZ.Basket.Api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/baskets")]
[OpenApiTag("baskets", Description = "Manage baskets.")]
//[ProtectedResource("baskets")]
//[AllowAnonymous]
[Authorize(Policy = Policies.REQUIRE_AUTHENTICATION)]
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
    [ProtectedResource(Resources.RESOURCE_USERS, Scopes.UPDATE_OWN)]
    public async Task<IActionResult> StoreBasket([FromBody] StoreBasketRequest request, CancellationToken cancellationToken)
    {
        var cmd = _mapper.Map<StoreBasketCommand>(request);

        var result = await _sender.Send(cmd, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }

    [HttpPost("product-model-item")]
    [ProtectedResource(Resources.RESOURCE_USERS, Scopes.UPDATE_OWN)]
    public async Task<IActionResult> StoreBasketItem([FromBody] StoreBasketItemRequest request, CancellationToken cancellationToken)
    {
        var cmd = _mapper.Map<StoreBasketItemCommand>(request);

        var result = await _sender.Send(cmd, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }

    [HttpPost("sync")]
    [ProtectedResource(Resources.RESOURCE_USERS, Scopes.UPDATE_OWN)]
    public async Task<IActionResult> SyncBasket([FromBody] StoreBasketRequest request, CancellationToken cancellationToken)
    {
        var cmd = _mapper.Map<SyncBasketCommand>(request);

        var result = await _sender.Send(cmd, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }

    [HttpPost("event-item")]
    public async Task<IActionResult> StoreEventItem([FromBody] StoreEventItemRequest request, CancellationToken cancellationToken)
    {
        var cmd = _mapper.Map<StoreEventItemCommand>(request);

        var result = await _sender.Send(cmd, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }

    [HttpGet()]
    [ProtectedResource(Resources.RESOURCE_USERS, Scopes.READ_OWN)]
    public async Task<IActionResult> GetBasket([FromQuery] GetBasketRequest request, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetBasketQuery(request._couponCode), cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }

    [HttpGet("checkout-items")]
    [ProtectedResource(Resources.RESOURCE_USERS, Scopes.READ_OWN)]
    public async Task<IActionResult> GetCheckoutBasket([FromQuery] GetBasketRequest request, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetCheckoutBasketQuery(request._couponCode), cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }

    [HttpPost("proceed-checkout")]
    [ProtectedResource(Resources.RESOURCE_USERS, Scopes.UPDATE_OWN)]
    public async Task<IActionResult> ProceedCheckout([FromBody] ProceedCheckoutRequest request, CancellationToken cancellationToken)
    {
        var cmd = new ProceedCheckoutCommand();

        var result = await _sender.Send(cmd, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }

    [HttpPost("checkout")]
    [ProtectedResource(Resources.RESOURCE_USERS, Scopes.UPDATE_OWN)]
    public async Task<IActionResult> CheckoutBasket([FromBody] CheckoutBasketRequest request, CancellationToken cancellationToken)
    {
        var cmd = _mapper.Map<CheckoutBasketCommand>(request);

        var result = await _sender.Send(cmd, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }

    [HttpPost("checkout/blockchain")]
    [ProtectedResource(Resources.RESOURCE_USERS, Scopes.UPDATE_OWN)]
    public async Task<IActionResult> CheckoutBasketWithBlockchain([FromBody] CheckoutBasketWithBlockchainRequest request, CancellationToken cancellationToken)
    {
        var cmd = _mapper.Map<CheckoutBasketWithBlockchainCommand>(request);

        var result = await _sender.Send(cmd, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }



    [HttpDelete()]
    [ProtectedResource(Resources.RESOURCE_USERS, Scopes.DELETE_OWN)]
    public async Task<IActionResult> DeleteBasket(CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new DeleteBasketCommand(), cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }
}
