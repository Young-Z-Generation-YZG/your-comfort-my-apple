using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Asp.Versioning;
using Swashbuckle.AspNetCore.Filters;
using YGZ.Catalog.Application.Products.Commands.CreateProduct;
using YGZ.Catalog.Api.Common.Extensions;

namespace YGZ.Catalog.Api.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
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
    //[Consumes("multipart/form-data")]
    [SwaggerRequestExample(typeof(CreateProductCommand), typeof(CreateProductExample))]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductCommand request, CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(request, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }
}
