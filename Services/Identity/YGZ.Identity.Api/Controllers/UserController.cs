using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using YGZ.Identity.Application.Users.Queries.GetProfile;
using YGZ.BuildingBlocks.Shared.Extensions;
using Keycloak.AuthServices.Authorization;
using Microsoft.AspNetCore.Authorization;
using static YGZ.BuildingBlocks.Shared.Constants.AuthorizationConstants;

namespace YGZ.Identity.Api.Controllers;

[ApiController]
//[Authorize(Roles = "[Role] USER")]
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
    [Authorize(Policy = Policies.RequireClientRole)]
    [ProtectedResource("profiles", "profile:read:own")]
    public async Task<IActionResult> GetProfile(CancellationToken cancellationToken)
    {
        var query = new GetProfileQuery();

        var result = await _sender.Send(query, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }
}



