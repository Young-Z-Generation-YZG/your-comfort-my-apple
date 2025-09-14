using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using YGZ.BuildingBlocks.Shared.Extensions;
using YGZ.Catalog.Api.Contracts.IPhone16;
using YGZ.Catalog.Api.Contracts.IphoneRequest;
using YGZ.Catalog.Api.Contracts.IPhoneRequest;
using YGZ.Catalog.Api.Contracts.modelRequest;
using YGZ.Catalog.Application.IPhone.Commands.CreateIphoneModel;
using YGZ.Catalog.Application.IPhone.Queries.GetModels;
using YGZ.Catalog.Application.IPhone16.Commands.CreateIPhone16Detail;
using YGZ.Catalog.Application.IPhone16.Queries.GetIPhone16Models;
using YGZ.Catalog.Application.IPhone16.Queries.GetIPhonePromotions;
using YGZ.Catalog.Application.IPhone16.Queries.GetIPhonesByModelSlug;

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
    public async Task<IActionResult> GetIPhone16WithPromotion([FromQuery] GetIPhonePromotionRequest request, CancellationToken cancellationToken)
    {
        var query = _mapper.Map<GetIPhonePromotionsQuery>(request);

        var result = await _sender.Send(query, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }

    [HttpGet("models")]
    public async Task<IActionResult> GetModelsWithPromotion([FromQuery] GetModelsRequest request, CancellationToken cancellationToken)
    {
        var query = _mapper.Map<GetModelsQuery>(request);

        var result = await _sender.Send(query, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }

    [HttpGet("{modelSlug}/models")]
    public async Task<IActionResult> GetModelBySlug([FromRoute] string modelSlug, CancellationToken cancellationToken)
    {
        var query = new GetModelsBySlugQuery(modelSlug);

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
