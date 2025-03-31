
using MediatR;
using Microsoft.AspNetCore.Mvc;
using YGZ.Identity.Application.Emails;
using YGZ.BuildingBlocks.Shared.Extensions;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using NSwag.Annotations;

namespace YGZ.Identity.Api.Controllers;

[ApiVersion(1)]
[Route("api/v{version:apiVersion}/sample")]
[OpenApiTag("Test Controller", Description = "Sample request.")]
[AllowAnonymous]
public class SampleController : ApiController
{
    private readonly ISender _sender;

    public SampleController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("test-send-email")]
    public async Task<IActionResult> TestSendEmail(CancellationToken cancellationToken)
    {
        var cmd = new TestSendEmailCommand("lov3rinve146@gmail.com", "Test Subject", "Test Body");

        var result = await _sender.Send(cmd, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }
}
