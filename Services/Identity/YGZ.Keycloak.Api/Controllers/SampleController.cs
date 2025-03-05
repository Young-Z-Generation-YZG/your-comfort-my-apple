using YGZ.Keycloak.Application.Emails;
using YGZ.BuildingBlocks.Shared.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace YGZ.Keycloak.Api.Controllers;


[ApiController]
[Route("samples")]
[OpenApiTag("samples", Description = "Test feature.")]
[AllowAnonymous]
public class SampleController : ApiController
{
    private readonly ISender _sender;
    public SampleController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("send")]
    public async Task<IActionResult> SendMail([FromBody] TestSendEmailCommand request, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(request, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }
}
