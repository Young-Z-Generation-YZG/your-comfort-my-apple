using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Asp.Versioning;
using YGZ.Catalog.Api.Common.Extensions;
using YGZ.Catalog.Application.Products.Commands.CreateProduct;
using Microsoft.AspNetCore.Authorization;

namespace YGZ.Catalog.Api.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion(1)]
public class ProductController : ApiController
{
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public ProductController(ISender sender, IMapper mapper)
    {
        _sender = sender;
        _mapper = mapper;
    }

    [AllowAnonymous]
    [HttpPost()]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductCommand request, CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(request, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }
}
