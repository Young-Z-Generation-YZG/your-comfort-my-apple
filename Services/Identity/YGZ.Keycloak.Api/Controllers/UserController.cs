using Keycloak.AuthServices.Authorization;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using YGZ.Keycloak.Application.Users.Queries;
using YGZ.BuildingBlocks.Shared.Extensions;

namespace YGZ.Keycloak.Api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/users")]
[OpenApiTag("users", Description = "Manage users.")]
[ProtectedResource("profiles")]
public class UserController : ApiController
{
    private readonly ILogger<UserController> _logger;
    private readonly ISender _sender;

    public UserController(ILogger<UserController> logger, ISender sender)
    {
        _logger = logger;
        _sender = sender;
    }

    [HttpGet("profiles")]
    [OpenApiOperation("[profiles:read]", "")]
    [ProtectedResource("profiles", "profile:read:own")]
    public async Task<IActionResult> GetProfile(CancellationToken cancellationToken)
    {
        var query = new GetProfileQuery();

        var result = await _sender.Send(query, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }

    //[HttpPut("profiles")]
    //[OpenApiOperation("[profiles:update]", "")]
    //[ProtectedResource("profiles", "profile:update:own")]
    //public async Task<ActionResult<IEnumerable<string>>> UpdateProfile(CancellationToken cancellationToken)
    //{
    //    return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    //}
}
