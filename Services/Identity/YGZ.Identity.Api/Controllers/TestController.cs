
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

  [HttpGet("test-logging-throw-exception")]
  public async Task<IActionResult> TestLoggingThrowException(CancellationToken cancellationToken)
  {
    //_logger.LogInformation("1. Starting test logging {@RequestName},{@DateTimeUtc}", typeof(TestController).Name, DateTime.UtcNow);
    //_logger.LogError("2. Starting test logging {@RequestName},{@DateTimeUtc}", typeof(TestController).Name, DateTime.UtcNow);

    throw new NotImplementedException();

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
