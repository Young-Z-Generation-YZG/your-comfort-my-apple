using Asp.Versioning;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using YGZ.BuildingBlocks.Shared.Extensions;
using YGZ.Keycloak.Api.Contracts;
using YGZ.Keycloak.Application.Auths.Commands.Login;


namespace YGZ.Keycloak.Api.Controllers;

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
        await Task.CompletedTask;

        _logger.LogInformation("Register request - Email: {Email}, Password: {Password}",
            request.Email, request.Password);

        return Ok("OK request");
    }
}
