using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace YGZ.Keycloak.Api.Controllers;

public class ErrorController : ControllerBase
{
    [Route("error")]
    [AllowAnonymous]
    public IActionResult Error()
    {
        Exception? exception = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;

        return Problem(exception?.Message);
    }
}
