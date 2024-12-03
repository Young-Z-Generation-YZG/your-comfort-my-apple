using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Swashbuckle.AspNetCore.Filters;
using Asp.Versioning;
using YGZ.Catalog.Application.Products.Commands.CreateProduct;
using YGZ.Catalog.Api.Common.Extensions;
using YGZ.Catalog.Contracts.Products;
using YGZ.Catalog.Api.Common.SwaggerExamples.Producs;
using YGZ.Catalog.Application.Products.Commands.CreateProductItem;
using YGZ.Catalog.Application.Products.Queries.GetAllProducts;
using YGZ.Catalog.Application.Products.Queries.GetProductById;
using YGZ.Catalog.Application.Products.Commands.DeleteProductById;
using YGZ.Catalog.Application.Products.Queries.GetProductBySlug;

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
    /// [DONE] Get ALL products
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> GetProducts(CancellationToken cancellationToken = default)
    {
        var query = new GetAllProductsQuery();

        var result = await _mediator.Send(query, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }

    /// <summary>
    /// [DONE] Get product by slug
    /// </summary>
    /// <returns></returns>
    [HttpGet("{slug}")]
    public async Task<IActionResult> GetProductById(string slug, CancellationToken cancellationToken = default)
    {
        var query = new GetProductBySlugQuery(slug);

        var result = await _mediator.Send(query, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }

    /// <summary>
    /// [DONE] Create a general product base model for product'items
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [SwaggerRequestExample(typeof(CreateProductRequest), typeof(CreateProductRequestExample))]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequest request, CancellationToken cancellationToken = default)
    {
        var cmd = _mapper.Map<CreateProductCommand>(request);

        var result = await _mediator.Send(cmd, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(string id, [FromBody] CreateProductRequest request, CancellationToken cancellationToken = default)
    {
        await Task.CompletedTask;
        return Ok();
        //return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(string id, CancellationToken cancellationToken = default)
    {
        var cmd = new DeleteProductByIdCommand(id);

        var result = await _mediator.Send(cmd, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }

    [HttpGet("product-items")]
    public async Task<IActionResult> GetProductItems(CancellationToken cancellationToken = default)
    {
        await Task.CompletedTask;
        return Ok();
        //return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }

    [HttpGet("product-items/{id}")]
    public async Task<IActionResult> GetProductItem(string id, CancellationToken cancellationToken = default)
    {
        await Task.CompletedTask;
        return Ok();
        //return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }

    /// <summary>
    /// [DONE] Create a Product detail base model for product'items
    /// </summary>
    /// <returns></returns>
    [HttpPost("product-items")]
    [SwaggerRequestExample(typeof(CreateProductItemRequest), typeof(CreateProductItemRequestExample))]
    public async Task<IActionResult> CreateProductItem([FromBody] CreateProductItemRequest request, CancellationToken cancellationToken = default)
    {
        var cmd = _mapper.Map<CreateProductItemCommand>(request);

        var result = await _mediator.Send(cmd, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }

    [HttpPut("product-items/{id}")]
    public async Task<IActionResult> UpdateProductItem(string id, [FromBody] CreateProductItemRequest request, CancellationToken cancellationToken = default)
    {
        await Task.CompletedTask;
        return Ok();
        //return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }

    [HttpDelete("product-items/{id}")]
    public async Task<IActionResult> DeleteProductItem(string id, CancellationToken cancellationToken = default)
    {
        await Task.CompletedTask;
        return Ok();
        //return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }
}
