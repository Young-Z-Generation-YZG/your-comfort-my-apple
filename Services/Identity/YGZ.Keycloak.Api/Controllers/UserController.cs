﻿using Keycloak.AuthServices.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace YGZ.Keycloak.Api.Controllers;

[ApiController]
[Route("users")]
[OpenApiTag("users", Description = "Manage users.")]
[ProtectedResource("users")]
public class UserController : ApiController
{
    [HttpGet("profile")]
    [OpenApiOperation("[user:profile]", "")]
    [ProtectedResource("users", "user:profile")]
    public async Task<ActionResult<IEnumerable<string>>> getProfile()
    {
        await Task.CompletedTask;

        return Ok("getProfile");
    }
}
