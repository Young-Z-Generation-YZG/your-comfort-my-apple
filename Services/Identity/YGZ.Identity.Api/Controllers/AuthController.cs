using Asp.Versioning;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using YGZ.BuildingBlocks.Shared.Extensions;
using YGZ.BuildingBlocks.Shared.Contracts.Auth;
using YGZ.Identity.Api.Contracts.Auth;
using YGZ.Identity.Api.Extensions;
using YGZ.Identity.Application.Auths.Commands.AccessOtpPage;
using YGZ.Identity.Application.Auths.Commands.ChangePassword;
using YGZ.Identity.Application.Auths.Commands.Login;
using YGZ.Identity.Application.Auths.Commands.Logout;
using YGZ.Identity.Application.Auths.Commands.RefreshToken;
using YGZ.Identity.Application.Auths.Commands.Register;
using YGZ.Identity.Application.Auths.Commands.ResetPassword;
using YGZ.Identity.Application.Auths.Commands.VerifyEmail;
using YGZ.Identity.Application.Auths.Commands.VerifyResetPassword;
using YGZ.Identity.Application.Auths.Queries.GetIdentity;
using YGZ.Identity.Domain.Core.Errors;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using static YGZ.BuildingBlocks.Shared.Constants.AuthorizationConstants;

namespace YGZ.Identity.Api.Controllers;

[Route("api/v{version:apiVersion}/auth")]
[ApiVersion(1)]
[OpenApiTag("auth", Description = "Manage auths.")]
[Authorize(Policy = Policies.REQUIRE_AUTHENTICATION)]
public class AuthController : ApiController
{
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public AuthController(ISender sender, IMapper mapper)
    {
        _sender = sender;
        _mapper = mapper;
    }

    [HttpGet("me")]
    // [ProtectedResource(Resources.RESOURCE_USERS, [
    //     Scopes.ALL,
    //     Scopes.READ_ANY,
    // ])]
    // [SwaggerHeader("X-TenantId", "Tenant identifier for multi-tenant operations", "", false)]
    public async Task<IActionResult> GetIdentity(CancellationToken cancellationToken)
    {
        var query = new GetIdentityQuery();

        var result = await _sender.Send(query, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }

    [HttpPost("login")]
    [SwaggerHeader("X-TenantId", "Tenant identifier for multi-tenant operations", "", false)]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var cmd = _mapper.Map<LoginCommand>(request);

        var result = await _sender.Send(cmd, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
    {
        var cmd = _mapper.Map<RegisterCommand>(request);

        var result = await _sender.Send(cmd, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }

    [HttpPost("change-password")]
    [AllowAnonymous]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request, CancellationToken cancellationToken)
    {
        var cmd = _mapper.Map<ChangePasswordCommand>(request);

        var result = await _sender.Send(cmd, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }

    [HttpGet("private/page/otp")]
    [AllowAnonymous]
    public async Task<IActionResult> GetOtpPage([FromQuery] AccessOtpRequest request, CancellationToken cancellationToken)
    {
        var cmd = _mapper.Map<AccessOtpPageCommand>(request);

        var result = await _sender.Send(cmd, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }

    [HttpPost("verification/email")]
    [AllowAnonymous]
    public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailRequest request, CancellationToken cancellationToken)
    {
        var cmd = _mapper.Map<VerifyEmailCommand>(request);

        var result = await _sender.Send(cmd, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }

    [HttpPost("reset-password")]
    [AllowAnonymous]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request, CancellationToken cancellationToken)
    {
        var cmd = _mapper.Map<ResetPasswordCommand>(request);

        var result = await _sender.Send(cmd, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }

    [HttpPost("verification/reset-password")]
    [AllowAnonymous]
    public async Task<IActionResult> ResetPassword([FromBody] VerifyResetPasswordRequest request, CancellationToken cancellationToken)
    {
        var cmd = _mapper.Map<VerifyResetPasswordCommand>(request);

        var result = await _sender.Send(cmd, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }

    private IActionResult HandleAuthFailure<TResponse>(Result<TResponse> result)
    {
        if (result.IsSuccess)
        {
            throw new InvalidOperationException("Result is not failure");
        }

        // HttpContext.Items.Add("error", result.Error);

        return Unauthorized(Problem(title: "Unauthorized", statusCode: 401).Value);
    }

    [HttpPost("refresh")]
    [AllowAnonymous]
    public async Task<IActionResult> RefreshAccessToken(CancellationToken cancellationToken)
    {
        // Extract Bearer token from Authorization header
        var authHeader = Request.Headers["Authorization"].FirstOrDefault();
        if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            var errorResult = Result<TokenResponse>.Failure(Errors.Auth.MissingAuthorizationHeader);
            return HandleAuthFailure(errorResult);
        }

        var refreshToken = authHeader.Substring("Bearer ".Length).Trim();
        
        if (string.IsNullOrWhiteSpace(refreshToken))
        {
            var errorResult = Result<TokenResponse>.Failure(Errors.Auth.MissingAuthorizationHeader);
            return HandleAuthFailure(errorResult);
        }

        var cmd = new RefreshTokenCommand { RefreshToken = refreshToken };

        var result = await _sender.Send(cmd, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleAuthFailure);
    }

    [HttpPost("logout")]
    [AllowAnonymous]
    public async Task<IActionResult> Logout(CancellationToken cancellationToken)
    {
        // Extract Bearer token from Authorization header
        var authHeader = Request.Headers["Authorization"].FirstOrDefault();
        if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            var errorResult = Result<bool>.Failure(Errors.Auth.MissingAuthorizationHeader);
            return HandleAuthFailure(errorResult);
        }

        var refreshToken = authHeader.Substring("Bearer ".Length).Trim();
        
        if (string.IsNullOrWhiteSpace(refreshToken))
        {
            var errorResult = Result<bool>.Failure(Errors.Auth.MissingAuthorizationHeader);
            return HandleAuthFailure(errorResult);
        }

        var cmd = new LogoutCommand { RefreshToken = refreshToken };

        var result = await _sender.Send(cmd, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleAuthFailure);
    }
}
