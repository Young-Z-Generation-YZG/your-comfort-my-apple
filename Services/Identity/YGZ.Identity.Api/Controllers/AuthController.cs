using System.Security.Claims;
using Asp.Versioning;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
using YGZ.BuildingBlocks.Shared.Extensions;
using YGZ.Identity.Api.Contracts;
using YGZ.Identity.Application.Auths.Commands.Login;
using YGZ.Identity.Application.Auths.Commands.Register;
using YGZ.Identity.Domain.Users;

namespace YGZ.Identity.Api.Controllers;

[Route("api/v{version:apiVersion}/auth")]
[ApiVersion(1)]
public class AuthController : ApiController
{
    private readonly ISender _sender;
    private readonly IMapper _mapper;
    private readonly SignInManager<User> _signInManager;

    public AuthController(ISender sender, IMapper mapper, SignInManager<User> signInManager)
    {
        _sender = sender;
        _mapper = mapper;
        _signInManager = signInManager;
    }

    [HttpPost("login")]
    [SwaggerRequestExample(typeof(LoginRequest), typeof(LoginRequestExample))]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var cmd = _mapper.Map<LoginCommand>(request);

        var result = await _sender.Send(cmd, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }

    [HttpPost("register")]
    [SwaggerRequestExample(typeof(RegisterRequest), typeof(RegisterRequestExample))]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken = default)
    {
        var cmd = _mapper.Map<RegisterCommand>(request);

        var result = await _sender.Send(cmd, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }

    [HttpGet("all-roles")]
    public IActionResult GetAllRoles()
    {
        return Ok();
    }

    [HttpGet("google-login")]
    [AllowAnonymous]
    public IActionResult GetExternalLoginUrl()
    {
        var properties = _signInManager.ConfigureExternalAuthenticationProperties("Google", "/api/v1/auth/google-response");

        return new ChallengeResult("Google", properties);
    }

    [HttpGet("google-response")]
    [AllowAnonymous]
    public async Task<IActionResult> GoogleResponse()
    {
        ExternalLoginInfo? info = await _signInManager.GetExternalLoginInfoAsync();

        if(info == null)
        {
            return BadRequest();
        }

        var result =await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);

        string[] userInfo = { info.Principal.FindFirst(ClaimTypes.NameIdentifier)!.Value, info.Principal.FindFirst(ClaimTypes.Email)!.Value };

        if (result.Succeeded)
        {
            return Redirect("https://example.com");
            return Ok(userInfo);
        }
        else
        {
            User newUser = new User
            {
                Email = info.Principal.FindFirst(ClaimTypes.Email)!.Value,
                UserName = info.Principal.FindFirst(ClaimTypes.Email)!.Value,
                FirstName = info.Principal.FindFirst(ClaimTypes.GivenName)!.Value,
                LastName = info.Principal.FindFirst(ClaimTypes.Surname)!.Value
            };

            IdentityResult identityResult = await _signInManager.UserManager.CreateAsync(newUser);

            if (identityResult.Succeeded)
            {
                identityResult = await _signInManager.UserManager.AddLoginAsync(newUser, info);
                if (identityResult.Succeeded)
                {
                    await _signInManager.SignInAsync(newUser, false);

                    return Redirect("https://example.com");
                    return Ok(userInfo);
                }
            }
        }

        return Redirect("https://example.com");
    }
}
