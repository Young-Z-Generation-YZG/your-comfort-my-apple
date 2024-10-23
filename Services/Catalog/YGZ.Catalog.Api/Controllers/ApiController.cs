using Microsoft.AspNetCore.Mvc;
using System.Net;
using YGZ.Catalog.Domain.Core.Abstractions.Result;
using YGZ.Catalog.Domain.Core.Abstractions.Result.Validations;

namespace YGZ.Catalog.Api.Controllers;

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

            return BadRequest(Problem(title: "BadRequest", statusCode: (int)HttpStatusCode.BadRequest).Value);
        }
        catch (InvalidCastException)
        {
            HttpContext.Items.Add("error", result.Error);

            return BadRequest(Problem(title: "BadRequest", statusCode: (int)HttpStatusCode.BadRequest).Value);
        }
    }
}
