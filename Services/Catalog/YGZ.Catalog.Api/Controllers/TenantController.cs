﻿using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using YGZ.BuildingBlocks.Shared.Extensions;
using YGZ.Catalog.Api.Contracts.TenantRequest;
using YGZ.Catalog.Application.Tenants.Commands;

namespace YGZ.Catalog.Api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/tenants")]
[OpenApiTag("Tenant Controllers", Description = "Manage tenants.")]
[AllowAnonymous]
public class TenantController : ApiController
{
    private readonly ILogger<TenantController> _logger;
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public TenantController(ILogger<TenantController> logger, ISender sender, IMapper mapper)
    {
        _logger = logger;
        _sender = sender;
        _mapper = mapper;
    }

    [HttpPost()]
    public async Task<IActionResult> CreateTenant([FromBody] CreateTenantRequest request, CancellationToken cancellationToken)
    {
        var cmd = _mapper.Map<CreateTenantCommand>(request);

        var result = await _sender.Send(cmd, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }
}
