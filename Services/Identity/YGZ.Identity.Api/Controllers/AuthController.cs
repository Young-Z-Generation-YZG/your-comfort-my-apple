using Asp.Versioning;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using YGZ.BuildingBlocks.Shared.Extensions;
using YGZ.Identity.Api.Contracts.Auth;
using YGZ.Identity.Application.Auths.Commands.AccessOtpPage;
using YGZ.Identity.Application.Auths.Commands.ChangePassword;
using YGZ.Identity.Application.Auths.Commands.Login;
using YGZ.Identity.Application.Auths.Commands.RefreshAccessToken;
using YGZ.Identity.Application.Auths.Commands.Register;
using YGZ.Identity.Application.Auths.Commands.ResetPassword;
using YGZ.Identity.Application.Auths.Commands.VerifyEmail;
using YGZ.Identity.Application.Auths.Commands.VerifyResetPassword;

namespace YGZ.Identity.Api.Controllers;

[Route("api/v{version:apiVersion}/auth")]
[ApiVersion(1)]
[OpenApiTag("auth", Description = "Manage auths.")]
[AllowAnonymous]
public class AuthController : ApiController
{
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public AuthController(ISender sender, IMapper mapper)
    {
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

    [HttpPost("verification/email")]
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

    [HttpPost("verification/reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] VerifyResetPasswordRequest request, CancellationToken cancellationToken)
    {
        var cmd = _mapper.Map<VerifyResetPasswordCommand>(request);

        var result = await _sender.Send(cmd, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshAccessToken([FromBody] RefreshAccessTokenRequest request, CancellationToken cancellationToken)
    {
        var cmd = _mapper.Map<RefreshAccessTokenCommand>(request);

        var result = await _sender.Send(cmd, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }
}
