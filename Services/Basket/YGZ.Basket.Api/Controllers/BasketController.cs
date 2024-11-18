using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using YGZ.Basket.Application.Baskets.Queries.GetBasket;
using YGZ.Basket.Api.Common.Extensions;

namespace YGZ.Basket.Api.Controllers;

[Route("api/v{version:apiVersion}/baskets")]
[ApiVersion(1)]
public class BasketController : ApiController
{
    private readonly ISender _mediator;

    public BasketController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetBasketByUserId(string userId, CancellationToken cancellationToken = default)
    {
        var query = new GetBasketByUserIdQuery(userId);

        var result = await _mediator.Send(query, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }

    /// <summary>
    /// Create Product
    /// </summary>
    /// <returns></returns>
    //[HttpPost]
    //[SwaggerRequestExample(typeof(CreateProductRequest), typeof(CreateProductRequestExample))]
    //public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequest request, CancellationToken cancellationToken = default)
    //{
    //    var cmd = _mapper.Map<CreateProductCommand>(request);

    //    //cmd.Files = request.Files; 

    //    var result = await _mediator.Send(cmd, cancellationToken);

    //    return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    //}
}