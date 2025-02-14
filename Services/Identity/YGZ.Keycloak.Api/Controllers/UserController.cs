using Keycloak.AuthServices.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace YGZ.Keycloak.Api.Controllers;

[ApiController]
[Route("users")]
[OpenApiTag("users", Description = "Manage users.")]
[ProtectedResource("catalogs")]
public class UserController : Controller
{
    [HttpGet(Name = nameof(getUsersAsync))]
    [OpenApiOperation("[catalog:list]", "")]
    [ProtectedResource("catalogs", "catalog:list")]
    public async Task<ActionResult<IEnumerable<string>>> getUsersAsync()
    {
        await Task.CompletedTask;

        return Ok("getUsersAsync");
    }
}
