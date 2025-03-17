using Asp.Versioning;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using YGZ.Identity.Api.Contracts;
using YGZ.Identity.Application.Auths.Commands.Login;
using YGZ.Identity.Application.Auths.Commands.Register;
using YGZ.BuildingBlocks.Shared.Extensions;

namespace YGZ.Identity.Api.Controllers;

[Route("api/v{version:apiVersion}/auth")]
[ApiVersion(1)]
[OpenApiTag("auth", Description = "Manage auths.")]
[AllowAnonymous]
public class AuthController : ApiController
{
    private readonly ISender _sender;
    private readonly IMapper _mapper;
    private readonly ILogger<AuthController> _logger;

    public AuthController(ILogger<AuthController> logger, ISender sender, IMapper mapper)
    {
        _logger = logger;
        _sender = sender;
        _mapper = mapper;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var cmd = _mapper.Map<LoginCommand>(request);

        var result = await _sender.Send(cmd, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
    {
        var cmd = _mapper.Map<RegisterCommand>(request);

        var result = await _sender.Send(cmd, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }
}
