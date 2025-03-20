using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using YGZ.Catalog.Api.Contracts.IPhone16;
using YGZ.BuildingBlocks.Shared.Extensions;
using YGZ.Catalog.Application.IPhone16.Commands.CreateIPhone16Model;
using YGZ.Catalog.Application.IPhone16.Commands.CreateIPhone16Detail;
using YGZ.Catalog.Application.Products.Queries.GetProductBySlug;
using YGZ.Catalog.Application.IPhone16.Queries.GetIPhonesByModelSlug;

namespace YGZ.Catalog.Api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/products/iphone-16")]
[OpenApiTag("iPhone 16 Controllers", Description = "Manage products.")]
[AllowAnonymous]
public class IPhone16Controller : ApiController
{
    private readonly ILogger<IPhone16Controller> _logger;
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public IPhone16Controller(ILogger<IPhone16Controller> logger, ISender sender, IMapper mapper)
    {
        _logger = logger;
        _sender = sender;
        _mapper = mapper;
    }

    [HttpGet("models/{modelSlug}/products")]
    public async Task<IActionResult> GetProductModelBySlug([FromRoute] string modelSlug, CancellationToken cancellationToken)
    {
        var query = new GetIPhonesByModelSlugQuery(modelSlug);

        var result = await _sender.Send(query, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }

    [HttpPost("models")]
    public async Task<IActionResult> CreateIPhone16ModelItem([FromBody] CreateIPhone16ModelRequest request, CancellationToken cancellationToken)
    {
        var cmd = _mapper.Map<CreateIPhone16ModelCommand>(request);

        var result = await _sender.Send(cmd, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }

    [HttpPost("details")]
    public async Task<IActionResult> CreateIPhone16DetailItem([FromBody] CreateIPhone16DetailRequest request, CancellationToken cancellationToken)
    {
        var cmd = _mapper.Map<CreateIPhone16DetailsCommand>(request);

        var result = await _sender.Send(cmd, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }
}
