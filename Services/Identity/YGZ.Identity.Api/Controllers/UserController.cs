using Asp.Versioning;
using Keycloak.AuthServices.Authorization;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using YGZ.BuildingBlocks.Shared.Extensions;
using YGZ.Identity.Api.Contracts.Addresses;
using YGZ.Identity.Api.Contracts.Profiles;
using YGZ.Identity.Application.Users.Commands.AddAddress;
using YGZ.Identity.Application.Users.Commands.DeleteAddress;
using YGZ.Identity.Application.Users.Commands.SetDefaultAddress;
using YGZ.Identity.Application.Users.Commands.UpdateAddress;
using YGZ.Identity.Application.Users.Commands.UpdateProfile;
using YGZ.Identity.Application.Users.Queries.GetAddresses;
using YGZ.Identity.Application.Users.Queries.GetProfile;

namespace YGZ.Identity.Api.Controllers;

//[Authorize(Roles = "[Role] USER")]
[Route("api/v{version:apiVersion}/users")]
[ApiVersion(1)]
[OpenApiTag("users", Description = "Manage users.")]
[ProtectedResource("profiles")]
public class UserController : ApiController
{
    private readonly ILogger<UserController> _logger;
    private readonly IMapper _mapper;
    private readonly ISender _sender;

    public UserController(ILogger<UserController> logger, ISender sender, IMapper mapper)
    {
        _logger = logger;
        _sender = sender;
        _mapper = mapper;
    }

    //[Authorize(Policy = Policies.RequireClientRole)]
    //[OpenApiOperation("[profiles:read]", "")]
    [HttpGet("me")]
    [ProtectedResource("profiles", "profile:read:own")]
    [AllowAnonymous]
    public async Task<IActionResult> GetMe(CancellationToken cancellationToken)
    {
        var query = new GetMeQuery();

        var result = await _sender.Send(query, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }

    [HttpGet("addresses")]
    [ProtectedResource("profiles", "profile:read:own")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAddresses(CancellationToken cancellationToken)
    {
        var query = new GetAddressesQuery();

        var result = await _sender.Send(query, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }


    [HttpPost("addresses")]
    [ProtectedResource("profiles", "profile:read:own")]
    [AllowAnonymous]
    public async Task<IActionResult> AddAddress([FromBody] AddAddressRequest request, CancellationToken cancellationToken)
    {
        var cmd = _mapper.Map<AddAddressCommand>(request);

        var result = await _sender.Send(cmd, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }

    [HttpPut("addresses/{addressId}")]
    [AllowAnonymous]
    public async Task<IActionResult> UpdateAddress([FromRoute] string addressId, [FromBody] UpdateAddressRequest request, CancellationToken cancellationToken)
    {
        var cmd = _mapper.Map<UpdateAddressCommand>(request);
        cmd.AddressId = addressId;

        var result = await _sender.Send(cmd, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }

    [HttpPatch("addresses/{addressId}/is-default")]
    [AllowAnonymous]
    public async Task<IActionResult> SetDefaultAddress([FromRoute] string addressId, CancellationToken cancellationToken)
    {
        var cmd = new SetDefaultAddressCommand(addressId);

        var result = await _sender.Send(cmd, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }

    [HttpPut("profiles")]
    [AllowAnonymous]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest request, CancellationToken cancellationToken)
    {
        var cmd = _mapper.Map<UpdateProfileCommand>(request);

        var result = await _sender.Send(cmd, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }

    [HttpDelete("addresses/{addressId}")]
    [AllowAnonymous]
    public async Task<IActionResult> DeleteAddress([FromRoute] string addressId, CancellationToken cancellationToken)
    {
        var cmd = new DeleteAddressCommand(addressId);

        var result = await _sender.Send(cmd, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }
}



