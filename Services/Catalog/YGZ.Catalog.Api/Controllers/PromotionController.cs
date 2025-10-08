using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using YGZ.BuildingBlocks.Shared.Extensions;
using YGZ.Catalog.Application.Promotions.Queries.GetPromotionItems;

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

    //[HttpGet("events")]
    //public async Task<IActionResult> GetPromotionEvents(CancellationToken cancellationToken)
    //{
    //    var query = new GetPromotionEventsQuery();

    //    var result = await _sender.Send(query, cancellationToken);

    //    return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    //}

    //[HttpPost("events")]
    //public async Task<IActionResult> CreatePromotionEvent([FromBody] CreatePromotionEventRequest request, CancellationToken cancellationToken)
    //{
    //    var command = _mapper.Map<CreatePromotionEventCommand>(request);

    //    var result = await _sender.Send(command, cancellationToken);

    //    return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    //}

    //[HttpPost("global-events")]
    //public async Task<IActionResult> CreatePromotionEventGlobal([FromBody] CreatePromotionGlobalRequest request, CancellationToken cancellationToken)
    //{
    //    var command = _mapper.Map<CreatePromotionGlobalCommand>(request);

    //    var result = await _sender.Send(command, cancellationToken);

    //    return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    //}

    [HttpGet("event/event-with-items")]
    public async Task<IActionResult> GetEventWithItems(CancellationToken cancellationToken)
    {
        var query = new GetEventWithItemsQuery();

        var result = await _sender.Send(query, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }


    //[HttpGet("coupons")]
    //public async Task<IActionResult> GetPromotionCoupons(CancellationToken cancellationToken)
    //{
    //    var query = new GetPromotionCouponsQuery();

    //    var result = await _sender.Send(query, cancellationToken);

    //    return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    //}

    //[HttpGet("coupons/{couponCode}")]
    //public async Task<IActionResult> GetPromotionCouponByCode([FromRoute] string couponCode, CancellationToken cancellationToken)
    //{
    //    var query = new GetPromotionCouponByCodeQuery(couponCode);

    //    var result = await _sender.Send(query, cancellationToken);

    //    return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    //}


    //[HttpGet("events/active")]
    //public async Task<IActionResult> GetActivePromotionEvent(CancellationToken cancellationToken)
    //{
    //    var query = new GetActivePromotionEventQuery();

    //    var result = await _sender.Send(query, cancellationToken);

    //    return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    //}



    //[HttpPatch("events/{eventId}/state")]
    //public async Task<IActionResult> UpdatePromotionEventState([FromRoute] Guid eventId, [FromBody] UpdatePromotionEventRequest request, CancellationToken cancellationToken)
    //{
    //    var command = _mapper.Map<UpdatePromotionEventCommand>(request);
    //    command.EventId = eventId;
    //    var result = await _sender.Send(command, cancellationToken);
    //    return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    //}
}
