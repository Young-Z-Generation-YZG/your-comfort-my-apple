using Asp.Versioning;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using YGZ.BuildingBlocks.Shared.Extensions;
using YGZ.Identity.Api.Contracts.Tenants;
using YGZ.Identity.Application.Tenants.CreateTenantUser;

namespace YGZ.Identity.Api.Controllers;

[ApiVersion(1)]
[Route("api/v{version:apiVersion}/tenants")]
[OpenApiTag("Tenant Controller", Description = "Tenant request.")]
public class TenantController : ApiController
{
    private readonly ISender _sender;
    private readonly IMapper _mapper;
    private readonly ILogger<TenantController> _logger;

    public TenantController(ISender sender,
                            IMapper mapper,
                            ILogger<TenantController> logger)
    {
        _sender = sender;
        _logger = logger;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<IActionResult> CreateTenantUser([FromBody] CreateTenantUserRequest request, CancellationToken cancellationToken)
    {
        var cmd = _mapper.Map<CreateTenantUserCommand>(request);

        var result = await _sender.Send(cmd, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }

}
