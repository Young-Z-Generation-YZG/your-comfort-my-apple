using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using YGZ.BuildingBlocks.Shared.Extensions;
using YGZ.Ordering.Api.Contracts;
using YGZ.Ordering.Application.Orders.Queries.RevenuesByTenants;
using YGZ.Ordering.Application.Orders.Queries.RevenuesByYears;
using YGZ.Ordering.Application.Orders.Queries.RevenuesQuery;

namespace YGZ.Ordering.Api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/orders/dashboard")]
[OpenApiTag("dashboard", Description = "Dashboard.")]
[AllowAnonymous]
public class DashboardController : ApiController
{
    private readonly ILogger<DashboardController> _logger;
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public DashboardController(ILogger<DashboardController> logger, ISender sender, IMapper mapper)
    {
        _logger = logger;
        _sender = sender;
        _mapper = mapper;
    }

    [HttpGet("revenues")]
    public async Task<IActionResult> GetRevenues([FromQuery] RevenuesRequest request, CancellationToken cancellationToken)
    {
        var query = new GetRevenuesQuery
        {
            StartDate = request._startDate,
            EndDate = request._endDate
        };

        var result = await _sender.Send(query, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }

    [HttpGet("revenues/years")]
    public async Task<IActionResult> GetRevenuesByYears([FromQuery] RevenuesByYearsRequest request, CancellationToken cancellationToken)
    {
        var query = new RevenuesByYearsQuery
        {
            Years = request._years
        };

        var result = await _sender.Send(query, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }

    [HttpGet("revenues/tenants")]
    public async Task<IActionResult> GetRevenuesByTenants([FromQuery] RevenuesByTenantsRequest request, CancellationToken cancellationToken)
    {
        var query = new RevenuesByTenantsQuery
        {
            Tenants = request._tenants,
            StartDate = request._startDate,
            EndDate = request._endDate
        };

        var result = await _sender.Send(query, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }
}
