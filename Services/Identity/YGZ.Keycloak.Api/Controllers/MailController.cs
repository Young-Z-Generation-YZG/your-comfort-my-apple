using Keycloak.AuthServices.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using YGZ.Keycloak.Application.Abstractions.Mails;

namespace YGZ.Keycloak.Api.Controllers;


[ApiController]
[Route("mails")]
[OpenApiTag("mails", Description = "Mail feature.")]
[AllowAnonymous]
public class MailController : ApiController
{
    private readonly IMailService _mailService;

    public MailController(IMailService mailService)
    {
        _mailService = mailService;
    }

    [HttpPost("send")]
    public async Task<ActionResult> SendMail(string receptor, string subject, string body)
    {
        await _mailService.SendMailAsync(receptor, subject, body);

        return Ok();
    }
}
