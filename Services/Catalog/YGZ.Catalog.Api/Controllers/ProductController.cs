using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Asp.Versioning;
using YGZ.Catalog.Application.Products.Commands.CreateProduct;
using YGZ.Catalog.Api.Common.Extensions;
using YGZ.Catalog.Contracts.Products;
using Swashbuckle.AspNetCore.Filters;
using YGZ.Catalog.Api.Common.SwaggerExamples.Producs;
using YGZ.Catalog.Application.Products.Commands.CreateProductItem;

namespace YGZ.Catalog.Api.Controllers;

[Route("api/v{version:apiVersion}/products")]
[ApiVersion(1)]
public class ProductController : ApiController
{
    private readonly ISender _mediator;
    private readonly IMapper _mapper;

    public ProductController(ISender mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    /// <summary>
    /// Create Product
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [SwaggerRequestExample(typeof(CreateProductRequest), typeof(CreateProductRequestExample))]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequest request, CancellationToken cancellationToken = default)
    {
        var cmd = _mapper.Map<CreateProductCommand>(request);

        //cmd.Files = request.Files; 

        var result = await _mediator.Send(cmd, cancellationToken);

        return result.Match(onSuccess: result => Ok(_mapper.Map<CreateProductResponse>(result)), onFailure: HandleFailure);
    }

    [HttpPost("product-items")]
    [SwaggerRequestExample(typeof(CreateProductItemRequest), typeof(CreateProductItemRequestExample))]
    public async Task<IActionResult> CreateProductItem([FromBody] CreateProductItemRequest request, CancellationToken cancellationToken = default)
    {
        var cmd = _mapper.Map<CreateProductItemCommand>(request);

        var result = await _mediator.Send(cmd, cancellationToken);

        return result.Match(onSuccess: result => Ok(_mapper.Map<CreateProductItemResponse>(result)), onFailure: HandleFailure);
    }
}
