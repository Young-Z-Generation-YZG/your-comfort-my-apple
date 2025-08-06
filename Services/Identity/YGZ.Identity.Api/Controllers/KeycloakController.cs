using Asp.Versioning;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using YGZ.Identity.Api.Contracts.Auth.Keycloak;
using YGZ.Identity.Application.Keycloak.Commands;
using YGZ.BuildingBlocks.Shared.Extensions;

namespace YGZ.Identity.Api.Controllers;

[Route("api/v{version:apiVersion}/keycloak")]
[ApiVersion(1)]
[OpenApiTag("keycloak", Description = "keycloak auths.")]
public class KeycloakController : ApiController
{
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public KeycloakController(ISender sender, IMapper mapper)
    {
        _sender = sender;
        _mapper = mapper;
    }

    [HttpPost("authorization-code")]
    public async Task<IActionResult> LoginAsAuthorizationCode([FromBody] AuthorizationCodeRequest request, CancellationToken cancellationToken)
    {
        var cmd = _mapper.Map<AuthorizationCodeCommand>(request);

        var result = await _sender.Send(cmd, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }
}
