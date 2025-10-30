using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using YGZ.BuildingBlocks.Shared.Extensions;
using YGZ.Catalog.Api.Contracts.IphoneRequest;
using YGZ.Catalog.Application.Inventory.Queries.GetWarehouses;

namespace YGZ.Catalog.Api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/inventory")]
[OpenApiTag("Inventory Controllers", Description = "Manage inventory.")]
[AllowAnonymous]
public class InventoryController : ApiController
{
    private readonly ILogger<InventoryController> _logger;
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public InventoryController(ILogger<InventoryController> logger, ISender sender, IMapper mapper)
    {
        _logger = logger;
        _sender = sender;
        _mapper = mapper;
    }

    [HttpGet("warehouse/admin")]
    public async Task<IActionResult> GetWarehouses([FromQuery] GetWarehousesRequest request, CancellationToken cancellationToken)
    {
        var query = _mapper.Map<GetWarehousesQuery>(request);

        var result = await _sender.Send(query, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }
}
