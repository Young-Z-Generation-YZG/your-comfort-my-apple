using System.Security.Claims;
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YGZ.Identity.Domain.Authorizations;

namespace YGZ.Identity.Api.Controllers;

[Route("api/v{version:apiVersion}/users")]
[ApiVersion(1)]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class UserController : ApiController
{
    [HttpGet("me")]
    //[AllowAnonymous]
    public async Task<IActionResult> GetMe(CancellationToken cancellationToken = default)
    {
        await Task.CompletedTask;

        return Ok();
    }
}
