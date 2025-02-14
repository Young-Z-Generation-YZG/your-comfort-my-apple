using Asp.Versioning;
using Keycloak.AuthServices.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YGZ.Identity.Domain.Authorizations;

namespace YGZ.Identity.Api.Controllers;

[Authorize]
[ApiVersion(1)]
[Route("api/v{version:apiVersion}/users")]
public class UserController : ApiController
{
    //[ProtectedResource("workspaces", "workspaces:read")]
    //[Authorize]
    [HttpGet("read")]
    public async Task<IActionResult> GetMe(CancellationToken cancellationToken = default)
    {
        await Task.CompletedTask;

        return Ok("Read profile");
    }

    //[HttpGet("policy")]
    ////[Authorize(Policy = "test_policy")]
    ////[Authorize(Policy = AuthorizationConstants.Policies.RequireToBeInKeycloakGroupAsReader)]
    //public async Task<IActionResult> TestPolicy(CancellationToken cancellationToken = default)
    //{
    //    await Task.CompletedTask;

    //    return Ok("Policy: policy.RequireResourceRoles(\"CREATE\")");
    //}

    [HttpPost("create")]
    //[Authorize(Roles = "CREATE")]
    //[ProtectedResource("workspaces", "workspaces:read")]
    public async Task<IActionResult> CreateProfile(CancellationToken cancellationToken = default)
    {
        await Task.CompletedTask;

        return Ok("Profile created");
    }


    [HttpPut("update")]
    //[Authorize(Roles = "UPDATE")]
    public async Task<IActionResult> UpdateProfile(CancellationToken cancellationToken = default)
    {
        await Task.CompletedTask;

        return Ok("Profile updated");
    }

    [HttpDelete("delete")]
    //[Authorize(Roles = "DELETE")]
    public async Task<IActionResult> DeleteProfile(CancellationToken cancellationToken = default)
    {
        await Task.CompletedTask;

        return Ok("Profile deleted");
    }
}
