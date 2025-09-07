using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using YGZ.BuildingBlocks.Shared.Extensions;
using YGZ.Catalog.Application.Reviews.Commands;
using YGZ.Catalog.Api.Contracts.ReviewRequest;
using static YGZ.BuildingBlocks.Shared.Constants.AuthorizationConstants;
using YGZ.Catalog.Application.Reviews.Queries.GetReviewsByOrder;

namespace YGZ.Catalog.Api.Controllers;

[Route("api/v{version:apiVersion}/reviews")]
[OpenApiTag("Review Controllers", Description = "Manage reviews.")]
public class ReviewController : ApiController
{
    private readonly ILogger<ReviewController> _logger;
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public ReviewController(ILogger<ReviewController> logger, ISender sender, IMapper mapper)
    {
        _logger = logger;
        _sender = sender;
        _mapper = mapper;
    }

    [HttpGet("{modelId}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetReviewsOfModel([FromRoute] string modelId, [FromQuery] GetReviewsByModelRequest request, CancellationToken cancellationToken)
    {
        var query = new GetReviewsByModelQuery(modelId)
        {
            Page = request._page,
            Limit = request._limit,
            SortBy = request._sortBy,
            SortOrder = request._sortOrder
        };

        var result = await _sender.Send(query, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }

    [HttpGet("orders/{orderId}")]
    [Authorize(Policy = Policies.RoleStaff)]
    public async Task<IActionResult> GetReviewsByOrder([FromRoute] string orderId, CancellationToken cancellationToken)
    {
        var query = new GetReviewsByOrderQuery(orderId);

        var result = await _sender.Send(query, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }

    [HttpPost()]
    [Authorize(Policy = Policies.RoleStaff)]
    public async Task<IActionResult> CreateView([FromBody] CreateReviewRequest request, CancellationToken cancellationToken)
    {
        var cmd = _mapper.Map<CreateReviewCommand>(request);

        var result = await _sender.Send(cmd, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }

    [HttpPut("{reviewId}")]
    [Authorize(Policy = Policies.RoleStaff)]
    public async Task<IActionResult> UpdateReview([FromRoute] string reviewId, [FromBody] UpdateReviewRequest request, CancellationToken cancellationToken)
    {
        var cmd = _mapper.Map<UpdateReviewCommand>(request);

        cmd.ReviewId = reviewId;

        var result = await _sender.Send(cmd, cancellationToken);
        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }

    [HttpDelete("{reviewId}")]
    [Authorize(Policy = Policies.RoleStaff)]
    public async Task<IActionResult> DeleteReview([FromRoute] string reviewId, CancellationToken cancellationToken)
    {
        var cmd = new DeleteReviewCommand(reviewId);

        var result = await _sender.Send(cmd, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }
}
