
using MediatR;
using Microsoft.AspNetCore.Mvc;
using YGZ.Identity.Application.Emails;
using YGZ.BuildingBlocks.Shared.Extensions;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using NSwag.Annotations;

namespace YGZ.Identity.Api.Controllers;

[ApiVersion(1)]
[Route("api/v{version:apiVersion}/test")]
[OpenApiTag("Test Controller", Description = "Test request.")]
[AllowAnonymous]
public class TestController : ApiController
{
    private readonly ILogger<TestController> _logger;
    private readonly ISender _sender;

    public TestController(ILogger<TestController> logger, ISender sender)
    {
        _sender = sender;
        _logger = logger;   
    }

    [HttpGet("test-logging")]
    public async Task<IActionResult> TestLogging(CancellationToken cancellationToken)
    {
        _logger.LogInformation("1. Starting test logging {@RequestName},{@DateTimeUtc}", typeof(TestController).Name, DateTime.UtcNow);
        _logger.LogError("2. Starting test logging {@RequestName},{@DateTimeUtc}", typeof(TestController).Name, DateTime.UtcNow);


        return Ok();
    }

    [HttpPost("test-send-email")]
    public async Task<IActionResult> TestSendEmail(CancellationToken cancellationToken)
    {
        var cmd = new TestSendEmailCommand("lov3rinve146@gmail.com", "Test Subject", "Test Body");

        var result = await _sender.Send(cmd, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }
}
