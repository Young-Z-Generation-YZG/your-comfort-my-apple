using Asp.Versioning;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using YGZ.Identity.Application.Auths.Commands.Login;
using YGZ.Identity.Application.Auths.Commands.Register;
using YGZ.BuildingBlocks.Shared.Extensions;
using YGZ.Identity.Application.Auths.Commands.VerifyEmail;
using YGZ.Identity.Application.Auths.Commands.AccessOtpPage;
using YGZ.Identity.Application.Auths.Commands.RefreshAccessToken;
using YGZ.Identity.Api.Contracts.Auth;
using YGZ.Identity.Application.Auths.Commands.ResetPassword;
using YGZ.Identity.Application.Auths.Commands.VerifyResetPassword;
using YGZ.Identity.Application.Auths.Commands.ChangePassword;

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

    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request, CancellationToken cancellationToken)
    {
        var cmd = _mapper.Map<ChangePasswordCommand>(request);

        var result = await _sender.Send(cmd, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }

    [HttpGet("private/page/otp")]
     public async Task<IActionResult> GetOtpPage([FromQuery] AccessOtpRequest request, CancellationToken cancellationToken)
    {
        var cmd = _mapper.Map<AccessOtpPageCommand>(request);

        var result = await _sender.Send(cmd, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }

    [HttpPost("email/verification")]
    public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailRequest request, CancellationToken cancellationToken)
    {
        var cmd = _mapper.Map<VerifyEmailCommand>(request);

        var result = await _sender.Send(cmd, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request, CancellationToken cancellationToken)
    {
        var cmd = _mapper.Map<ResetPasswordCommand>(request);

        var result = await _sender.Send(cmd, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }

    [HttpPost("reset-password/verification")]
    public async Task<IActionResult> ResetPassword([FromBody] VerifyResetPasswordRequest request, CancellationToken cancellationToken)
    {
        var cmd = _mapper.Map<VerifyResetPasswordCommand>(request);

        var result = await _sender.Send(cmd, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }

    [HttpPost("refresh/access-token")]
    public async Task<IActionResult> RefreshAccessToken([FromBody] RefreshAccessTokenRequest request, CancellationToken cancellationToken)
    {
        var cmd = _mapper.Map<RefreshAccessTokenCommand>(request);

        var result = await _sender.Send(cmd, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }
}
