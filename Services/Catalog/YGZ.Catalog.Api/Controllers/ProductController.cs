using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Asp.Versioning;
using Swashbuckle.AspNetCore.Filters;
using YGZ.Catalog.Application.Products.Commands.CreateProduct;
using YGZ.Catalog.Api.Common.Extensions;
using YGZ.Catalog.Contracts.Products;
using YGZ.Catalog.Api.Common.SwaggerExamples;

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
    //[SwaggerRequestExample(typeof(CreateProductRequest), typeof(CreateProductRequestExample))]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequest request, CancellationToken cancellationToken = default)
    {
        Console.WriteLine("request" + request);

        var cmd = _mapper.Map<CreateProductCommand>(request);

        //cmd.Files = request.Files;

        var result = await _mediator.Send(cmd, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }
}
