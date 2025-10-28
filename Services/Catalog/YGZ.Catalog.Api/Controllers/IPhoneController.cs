
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using YGZ.BuildingBlocks.Shared.Extensions;
using YGZ.Catalog.Api.Contracts.IphoneRequest;
using YGZ.Catalog.Application.Iphone.Queries.GetIphoneModelBySlug;
using YGZ.Catalog.Application.Iphone.Queries.GetIphoneModels;
using YGZ.Catalog.Application.IPhone.Commands.CreateIphoneModel;

namespace YGZ.Catalog.Api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/products/iphone")]
[OpenApiTag("iPhone Controllers", Description = "Manage iphones.")]
[AllowAnonymous]
public class IphoneController : ApiController
{
    private readonly ILogger<IphoneController> _logger;
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public IphoneController(ILogger<IphoneController> logger, ISender sender, IMapper mapper)
    {
        _logger = logger;
        _sender = sender;
        _mapper = mapper;
    }

    [HttpGet("{modelSlug}")]
    public async Task<IActionResult> GetIphoneModel([FromRoute] string modelSlug, CancellationToken cancellationToken)
    {
        var query = new GetIphoneModelBySlugQuery(modelSlug);

        var result = await _sender.Send(query, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }

    [HttpGet("models")]
    public async Task<IActionResult> GetModels([FromQuery] GetIphoneModelsRequest request, CancellationToken cancellationToken)
    {
        var query = _mapper.Map<GetIphoneModelsQuery>(request);

        var result = await _sender.Send(query, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }

    [HttpPost("iphone/models")]
    public async Task<IActionResult> CreateIphoneModel([FromBody] CreateIphoneModelRequest request, CancellationToken cancellationToken)
    {
        var cmd = _mapper.Map<CreateIphoneModelCommand>(request);

        var result = await _sender.Send(cmd, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }
}
