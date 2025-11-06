using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using YGZ.BuildingBlocks.Shared.Extensions;
using YGZ.Catalog.Api.Contracts.ProductModelRequest;
using YGZ.Catalog.Application.Products.Queries.GetPopularProducts;
using YGZ.Catalog.Application.Products.Queries.GetProductModels;
using YGZ.Catalog.Application.Products.Queries.GetRecommendationProducts;
using YGZ.Catalog.Application.Products.Queries.GetSuggestionProducts;

namespace YGZ.Catalog.Api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/products")]
[OpenApiTag("Product Controllers", Description = "Display products.")]
[AllowAnonymous]
public class ProductController : ApiController
{
    private readonly ILogger<ProductController> _logger;
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public ProductController(ILogger<ProductController> logger, ISender sender, IMapper mapper)
    {
        _logger = logger;
        _sender = sender;
        _mapper = mapper;
    }

    [HttpGet()]
    public async Task<IActionResult> GetProducts([FromQuery] GetProductModelsRequest request, CancellationToken cancellationToken)
    {
        var query = new GetProductModelsQuery
        {
            Page = request._page,
            Limit = request._limit,
            TextSearch = request._textSearch
        };

        var result = await _sender.Send(query, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }

    [HttpGet("popular")]
    public async Task<IActionResult> GetPopularProducts(CancellationToken cancellationToken)
    {
        var query = new GetPopularProductsQuery();

        var result = await _sender.Send(query, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }

    [HttpGet("newest")]
    public async Task<IActionResult> GetNewestProducts(CancellationToken cancellationToken)
    {
        var query = new GetNewestProductsQuery();

        var result = await _sender.Send(query, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }

    [HttpGet("suggestion")]
    public async Task<IActionResult> GetSuggestionProducts(CancellationToken cancellationToken)
    {
        var query = new GetSuggestionProductsQuery();

        var result = await _sender.Send(query, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }
}
