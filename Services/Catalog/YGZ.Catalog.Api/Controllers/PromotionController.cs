using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using YGZ.BuildingBlocks.Shared.Extensions;
using YGZ.Catalog.Api.Contracts.PromotionRequest;
using YGZ.Catalog.Application.Promotions.Events.UpdateEvent;
using YGZ.Catalog.Application.Promotions.Queries.GetEventDetails;
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
    private readonly IMapper _mapper;

    public PromotionController(ILogger<PromotionController> logger, ISender sender, IMapper mapper)
    {
        _logger = logger;
        _sender = sender;
        _mapper = mapper;
    }

    [HttpGet("events")]
    public async Task<IActionResult> GetEvents(CancellationToken cancellationToken)
    {
        var query = new GetEventsQuery();

        var result = await _sender.Send(query, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }

    [HttpGet("events/{eventId}")]
    public async Task<IActionResult> GetEventById([FromRoute] string eventId, CancellationToken cancellationToken)
    {
        var query = new GetEventDetailsQuery { EventId = eventId };

        var result = await _sender.Send(query, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }


    [HttpPatch("events/{eventId}")]
    public async Task<IActionResult> UpdateEvent([FromRoute] string eventId, [FromBody] UpdateEvenRequest request, CancellationToken cancellationToken)
    {
        var cmd = new UpdateEventCommand
        {
            EventId = eventId,
            Title = request.Title,
            Description = request.Description,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            AddEventItems = request.AddEventItems?.Select(item => new UpdateEventItemCommand
            {
                SkuId = item.SkuId,
                DiscountType = item.DiscountType,
                DiscountValue = item.DiscountValue,
                Stock = item.Stock
            }).ToList(),
            RemoveEventItemIds = request.RemoveEventItemIds
        };

        var result = await _sender.Send(cmd, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }
}
