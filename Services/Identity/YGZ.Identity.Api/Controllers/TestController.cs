
using Asp.Versioning;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using YGZ.BuildingBlocks.Shared.Extensions;
using YGZ.Identity.Api.Contracts.Auth;
using YGZ.Identity.Application.Auths.Commands.Login;
using YGZ.Identity.Application.Emails;

namespace YGZ.Identity.Api.Controllers;

[ApiVersion(1)]
[Route("api/v{version:apiVersion}/test")]
[OpenApiTag("Test Controller", Description = "Test request.")]
[AllowAnonymous]
public class TestController : ApiController
{
    private readonly ILogger<TestController> _logger;
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public TestController(ILogger<TestController> logger, ISender sender, IMapper mapper)
    {
        _sender = sender;
        _logger = logger;
        _mapper = mapper;
    }

    [HttpGet("test-logging")]
    public async Task<IActionResult> TestLoggingException(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Test logging information at {DateTimeUtc}", DateTime.UtcNow);
        _logger.LogWarning("Test logging warning at {DateTimeUtc}", DateTime.UtcNow);
        _logger.LogError("Test logging error at {DateTimeUtc}", DateTime.UtcNow);
        _logger.LogCritical("Test logging critical at {DateTimeUtc}", DateTime.UtcNow);
        _logger.LogTrace("Test logging trace at {DateTimeUtc}", DateTime.UtcNow);
        _logger.LogDebug("Test logging debug at {DateTimeUtc}", DateTime.UtcNow);

        return Ok();
    }

    [HttpGet("test-logging-throw-exception")]
    public async Task<IActionResult> TestLoggingThrowException(CancellationToken cancellationToken)
    {

        try
        {
            throw new NotImplementedException();
        }
        catch (Exception ex)
        {
            var parameters = new { email = "test@test.com", password = "password" };
            _logger.LogError(ex, "[Application Exception] Method: {MethodName}, Parameters: {@Parameters}, Error: {ErrorMessage}", nameof(TestLoggingThrowException), parameters, ex.Message);
            throw;
        }


        //_logger.LogInformation("1. Starting test logging {@Parameters}", parameters);
        //_logger.LogInformation("1. Starting test logging {@RequestName},{@DateTimeUtc}", typeof(TestController).Name, DateTime.UtcNow);
        //_logger.LogError("2. Starting test logging {@RequestName},{@DateTimeUtc}", typeof(TestController).Name, DateTime.UtcNow);

        //throw new NotImplementedException();

        return Ok();
    }

    [HttpGet("test-logging-return-result-error")]
    public async Task<IActionResult> TestLoggingReturnResultError([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var cmd = _mapper.Map<LoginCommand>(request);

        var result = await _sender.Send(cmd, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }

    [HttpGet("test-logging-fluent-validation")]
    public async Task<IActionResult> TestLoggingFluentValidation(CancellationToken cancellationToken)
    {
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
