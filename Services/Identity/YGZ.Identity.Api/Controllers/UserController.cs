using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using YGZ.Identity.Application.Users.Queries.GetProfile;
using YGZ.BuildingBlocks.Shared.Extensions;
using Asp.Versioning;
using Keycloak.AuthServices.Authorization;

namespace YGZ.Identity.Api.Controllers;

//[Authorize(Roles = "[Role] USER")]
[Route("api/v{version:apiVersion}/users")]
[ApiVersion(1)]
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

    //[Authorize(Policy = Policies.RequireClientRole)]
    //[OpenApiOperation("[profiles:read]", "")]
    [HttpGet("profiles")]
    [ProtectedResource("profiles", "profile:read:own")]
    public async Task<IActionResult> GetProfile(CancellationToken cancellationToken)
    {
        var query = new GetProfileQuery();

        var result = await _sender.Send(query, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }
}



