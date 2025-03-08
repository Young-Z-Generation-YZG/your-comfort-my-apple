using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using YGZ.Catalog.Api.Contracts;
using YGZ.BuildingBlocks.Shared.Extensions;
using Microsoft.AspNetCore.Authorization;
using YGZ.Catalog.Application.ProductItems.Commands.CreateProductItem;

namespace YGZ.Catalog.Api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/product-items")]
[OpenApiTag("product items", Description = "Manage product items.")]
//[ProtectedResource("profiles")]
[AllowAnonymous]
public class ProductItemController : ApiController
{
    private readonly ILogger<ProductItemController> _logger;
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public ProductItemController(ILogger<ProductItemController> logger, ISender sender, IMapper mapper)
    {
        _logger = logger;
        _sender = sender;
        _mapper = mapper;
    }

    [HttpPost()]
    public async Task<IActionResult> CreateProductItem([FromBody] CreateProductItemRequest request, CancellationToken cancellationToken)
    {
        var cmd = _mapper.Map<CreateProductItemCommand>(request);

        var result = await _sender.Send(cmd, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }
}
