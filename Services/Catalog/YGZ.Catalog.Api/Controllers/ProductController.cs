using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using YGZ.BuildingBlocks.Shared.Extensions;
using YGZ.Catalog.Api.Contracts;
using YGZ.Catalog.Application.Products.Queries.GetProductBySlug;
using YGZ.Catalog.Application.Products.Queries.GetProductsPagination;

namespace YGZ.Catalog.Api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/products")]
[OpenApiTag("Product Controllers", Description = "Manage products.")]
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
    public async Task<IActionResult> GetProducts([FromQuery] GetProductsRequest request, CancellationToken cancellationToken)
    {
        var query = _mapper.Map<GetProductsQuery>(request);

        var result = await _sender.Send(query, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }

    [HttpGet("{slug}/details")]
    public async Task<IActionResult> GetProductBySlug([FromRoute] string slug, CancellationToken cancellationToken)
    {
        var query = new GetProductBySlugQuery(slug);

        var result = await _sender.Send(query, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }

}
