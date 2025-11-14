using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using YGZ.BuildingBlocks.Shared.Extensions;
using YGZ.Catalog.Application.Promotions.Queries.GetEvents;

namespace YGZ.Catalog.Api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/promotions")]
[OpenApiTag("promotion Controllers", Description = "Manage promotions.")]
[AllowAnonymous]
public class PromotionController : ApiController
{
    private readonly ILogger<PromotionController> _logger;
    private readonly ISender _sender;

    public PromotionController(ILogger<PromotionController> logger, ISender sender)
    {
        _logger = logger;
        _sender = sender;
    }

    [HttpGet("events")]
    public async Task<IActionResult> GetEvents(CancellationToken cancellationToken)
    {
        var query = new GetEventsQuery();

        var result = await _sender.Send(query, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }

    // [HttpGet("{eventId}")]
    // public async Task<IActionResult> GetEventById([FromRoute] string eventId, CancellationToken cancellationToken)
    // {
    //     var query = new GetEventDetailsQuery { EventId = eventId };

    //     var result = await _sender.Send(query, cancellationToken);

    //     return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    // }
}
