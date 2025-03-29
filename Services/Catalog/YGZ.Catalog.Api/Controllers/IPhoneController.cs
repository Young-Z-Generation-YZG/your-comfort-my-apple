using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using YGZ.Catalog.Api.Contracts.IPhone16;
using YGZ.BuildingBlocks.Shared.Extensions;
using YGZ.Catalog.Application.IPhone16.Commands.CreateIPhone16Model;
using YGZ.Catalog.Application.IPhone16.Commands.CreateIPhone16Detail;
using YGZ.Catalog.Application.IPhone16.Queries.GetIPhonesByModelSlug;
using YGZ.Catalog.Application.IPhone16.Queries.GetIPhone16Models;
using YGZ.Catalog.Api.Contracts.IPhoneRequest;
using YGZ.Catalog.Application.IPhone16.Queries.GetIPhonePromotions;

namespace YGZ.Catalog.Api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/products/iphone")]
[OpenApiTag("iPhone Controllers", Description = "Manage iphones.")]
[AllowAnonymous]
public class IPhoneController : ApiController
{
    private readonly ILogger<IPhoneController> _logger;
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public IPhoneController(ILogger<IPhoneController> logger, ISender sender, IMapper mapper)
    {
        _logger = logger;
        _sender = sender;
        _mapper = mapper;
    }

    [HttpGet("promotions")]
    public async Task<IActionResult> GetIPhone16Promotion([FromQuery] GetIPhonePromotionRequest request, CancellationToken cancellationToken)
    {
        var query = _mapper.Map<GetIPhonePromotionsQuery>(request);

        var result = await _sender.Send(query, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }

    [HttpGet("iphone-16/models")]
    public async Task<IActionResult> GetIPhone16Models(CancellationToken cancellationToken)
    {
        var query = new GetIPhone16ModelsQuery();

        var result = await _sender.Send(query, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }

    [HttpPost("iphone-16/models")]
    public async Task<IActionResult> CreateIPhone16ModelItem([FromBody] CreateIPhone16ModelRequest request, CancellationToken cancellationToken)
    {
        var cmd = _mapper.Map<CreateIPhone16ModelCommand>(request);

        var result = await _sender.Send(cmd, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }

    [HttpGet("models/{modelSlug}/products")]
    public async Task<IActionResult> GetProductModelBySlug([FromRoute] string modelSlug, CancellationToken cancellationToken)
    {
        var query = new GetIPhonesByModelSlugQuery(modelSlug);

        var result = await _sender.Send(query, cancellationToken);

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
