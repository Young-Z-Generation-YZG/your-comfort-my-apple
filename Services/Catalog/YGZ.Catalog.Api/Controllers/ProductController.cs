using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using YGZ.Catalog.Api.Contracts;
using YGZ.Catalog.Application.ProductItems.Commands;
using YGZ.BuildingBlocks.Shared.Extensions;
using YGZ.Catalog.Application.Products.Commands.CreateProduct;

namespace YGZ.Catalog.Api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/products")]
[OpenApiTag("products", Description = "Manage products.")]
//[ProtectedResource("products")]
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

    [HttpPost()]
    public async Task<IActionResult> CreateProductItem([FromBody] CreateProductRequest request, CancellationToken cancellationToken)
    {
        var cmd = _mapper.Map<CreateProductCommand>(request);

        var result = await _sender.Send(cmd, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }
}
