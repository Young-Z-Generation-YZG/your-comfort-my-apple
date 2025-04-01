using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using YGZ.Catalog.Application.Promotions.Queries.GetActivePromotionEvent;
using YGZ.BuildingBlocks.Shared.Extensions;

namespace YGZ.Catalog.Api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/promotions")]
[OpenApiTag("promotion Controllers", Description = "Manage promotions.")]
[AllowAnonymous]
public class PromotionController : ApiController
{
    private readonly ILogger<PromotionController> _logger;
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public PromotionController(ILogger<PromotionController> logger, ISender sender, IMapper mapper)
    {
        _logger = logger;
        _sender = sender;
        _mapper = mapper;
    }

    [HttpGet("events")]
    public async Task<IActionResult> GetPromotionEvent(CancellationToken cancellationToken)
    {
        var query = new GetActivePromotionEventQuery();

        var result = await _sender.Send(query, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }
}
