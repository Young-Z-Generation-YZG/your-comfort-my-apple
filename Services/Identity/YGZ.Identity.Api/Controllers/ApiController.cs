using System.Net;
using Microsoft.AspNetCore.Mvc;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;

namespace YGZ.Identity.Api.Controllers;

[ApiController]
public class ApiController : ControllerBase
{
    protected IActionResult HandleFailure<TResponse>(Result<TResponse> result)
    {
        if (result.IsSuccess)
        {
            throw new InvalidOperationException("Result is not failure");
        }

        try
        {
            IValidationResult validationResult = (IValidationResult)result;

            HttpContext.Items.Add("errors", validationResult.Errors);
            HttpContext.Items.Add("error_type", "VALIDATION");

            return BadRequest(Problem(title: "BadRequest", statusCode: (int)HttpStatusCode.BadRequest).Value);
        }
        catch (InvalidCastException)
        {
            HttpContext.Items.Add("error", result.Error);
            HttpContext.Items.Add("error_type", "USER");

            return BadRequest(Problem(title: "BadRequest", statusCode: (int)HttpStatusCode.BadRequest).Value);
        }
    }
}