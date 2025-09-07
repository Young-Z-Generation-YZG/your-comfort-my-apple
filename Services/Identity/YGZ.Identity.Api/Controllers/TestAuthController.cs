
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using NSwag.Annotations;
using Microsoft.AspNetCore.Authorization;
using static YGZ.BuildingBlocks.Shared.Constants.AuthorizationConstants;
using Keycloak.AuthServices.Authorization;

namespace YGZ.Identity.Api.Controllers;

[ApiVersion(1)]
[Route("api/v{version:apiVersion}/test-auth")]
[OpenApiTag("Test Auth Controller", Description = "Test auth request.")]
public class TestAuthController : ApiController
{
    public TestAuthController() { }

    [HttpGet("role-based-access-control")]
    [Authorize(Policy = Policies.RoleStaff)]
    public async Task<IActionResult> TestRoleBasedAccessControl(CancellationToken cancellationToken)
    {
        return Ok();
    }

    [HttpGet("resource-based-access-control")]
    [ProtectedResource("test-resources")]
    public async Task<IActionResult> TestResourceBasedAccessControl(CancellationToken cancellationToken)
    {
        return Ok();
    }

    [HttpGet("scoped-based-access-control")]
    [ProtectedResource("test-scoped-resources", "test-resources:teasdasdasdst")]
    public async Task<IActionResult> TestScopedBasedAccessControl(CancellationToken cancellationToken)
    {
        return Ok();
    }
}
