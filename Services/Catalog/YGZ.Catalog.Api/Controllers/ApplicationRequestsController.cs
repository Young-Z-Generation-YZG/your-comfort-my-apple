using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using YGZ.BuildingBlocks.Shared.Extensions;
using YGZ.Catalog.Api.Contracts.SkuRequestRequest;
using YGZ.Catalog.Application.Requests.Commands.CreateSkuRequest;
using YGZ.Catalog.Application.Requests.Commands.UpdateSkuRequest;
using YGZ.Catalog.Application.Requests.Queries.GetSkuRequest;
using YGZ.Catalog.Application.Requests.Queries.GetSkuRequests;

namespace YGZ.Catalog.Api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/application-requests")]
[OpenApiTag("Application Request Controllers", Description = "Manage application requests.")]
[AllowAnonymous]
public class RequestsController : ApiController
{
    private readonly ILogger<RequestsController> _logger;
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public RequestsController(ILogger<RequestsController> logger, ISender sender, IMapper mapper)
    {
        _logger = logger;
        _sender = sender;
        _mapper = mapper;
    }

    [HttpGet()]
    public async Task<IActionResult> GetSkuRequests([FromQuery] GetSkuRequestsRequest request, CancellationToken cancellationToken)
    {
        var query = new GetSkuRequestsQuery
        {
            Page = request._page,
            Limit = request._limit,
            RequestState = request._requestState,
            TransferType = request._transferType,
            BranchId = request._branchId
        };

        var result = await _sender.Send(query, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetSkuRequest([FromRoute] string id, CancellationToken cancellationToken)
    {
        var query = new GetSkuRequestQuery(id);

        var result = await _sender.Send(query, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }

    [HttpPost()]
    public async Task<IActionResult> CreateSkuRequest([FromBody] CreateSkuRequestRequest request, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<CreateSkuRequestCommand>(request);

        var result = await _sender.Send(command, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSkuRequest([FromRoute] string id, [FromBody] UpdateSkuRequestRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateSkuRequestCommand(id, request.State);

        var result = await _sender.Send(command, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }
}
