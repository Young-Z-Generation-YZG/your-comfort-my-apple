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
using YGZ.Identity.Api.Contracts.Users;
using YGZ.Identity.Api.Extensions;
using YGZ.Identity.Application.Users.Commands.AddAddress;
using YGZ.Identity.Application.Users.Commands.DeleteAddress;
using YGZ.Identity.Application.Users.Commands.SetDefaultAddress;
using YGZ.Identity.Application.Users.Commands.UpdateAddress;
using YGZ.Identity.Application.Users.Commands.UpdateProfile;
using YGZ.Identity.Application.Users.Commands.UpdateProfileById;
using YGZ.Identity.Application.Users.Queries.GetAccount;
using YGZ.Identity.Application.Users.Queries.GetAddresses;
using YGZ.Identity.Application.Users.Queries.GetListUsers;
using YGZ.Identity.Application.Users.Queries.GetProfile;
using YGZ.Identity.Application.Users.Queries.GetUserById;
using YGZ.Identity.Application.Users.Queries.GetUsers;
using YGZ.Identity.Application.Users.Queries.GetUsersByAdmin;
using static YGZ.BuildingBlocks.Shared.Constants.AuthorizationConstants;

namespace YGZ.Identity.Api.Controllers;

[Route("api/v{version:apiVersion}/users")]
[ApiVersion(1)]
[OpenApiTag("users", Description = "Manage users.")]
[Authorize(Policy = Policies.REQUIRE_AUTHENTICATION)]
public class UserController : ApiController
{
    private readonly IMapper _mapper;
    private readonly ISender _sender;

    public UserController(ISender sender, IMapper mapper)
    {
        _sender = sender;
        _mapper = mapper;
    }

    [HttpGet("admin")]
    // [Authorize(Policy = Policies.GetUsers)]
    // [ProtectedResource(Resources.RESOURCE_USERS, [
    //     Scopes.ALL,
    //     Scopes.READ_ANY,
    // ])]
    [SwaggerHeader("X-TenantId", "Tenant identifier for multi-tenant operations", "", false)]
    public async Task<IActionResult> GetUsersByAdmin([FromQuery] GetUsersPaginationRequest request, CancellationToken cancellationToken)
    {
        var tenantId = Request.Headers["X-TenantId"].FirstOrDefault();

        var query = new GetUsersByAdminQuery
        {
            TenantId = tenantId,
            Page = request._page,
            Limit = request._limit,
            Email = request._email,
            FirstName = request._firstName,
            LastName = request._lastName,
            PhoneNumber = request._phoneNumber
        };

        var result = await _sender.Send(query, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }

    [HttpGet("")]
    public async Task<IActionResult> GetUsers([FromQuery] GetUsersRequest request, CancellationToken cancellationToken)
    {
        var query = new GetUsersQuery
        {
            Page = request._page,
            Limit = request._limit,
            Email = request._email,
            FirstName = request._firstName,
            LastName = request._lastName,
            PhoneNumber = request._phoneNumber
        };

        var result = await _sender.Send(query, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUserById([FromRoute] string userId, CancellationToken cancellationToken)
    {
        var query = new GetUserByIdQuery
        {
            UserId = userId
        };

        var result = await _sender.Send(query, cancellationToken);
        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }

    [HttpGet("list")]
    [AllowAnonymous]
    public async Task<IActionResult> GetListUsers([FromQuery] GetListUsersRequest request, CancellationToken cancellationToken)
    {
        var query = new GetListUsersQuery
        {
            Roles = request._roles
        };

        var result = await _sender.Send(query, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }

    [HttpGet("account")]
    public async Task<IActionResult> GetAccount(CancellationToken cancellationToken)
    {
        var query = new GetAccountQuery();

        var result = await _sender.Send(query, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }

    [HttpGet("me")]
    [SwaggerHeader("X-User-Language", "User's preferred language", "en-US", false)]
    [ProtectedResource(Resources.RESOURCE_USERS, Scopes.READ_OWN)]
    public async Task<IActionResult> GetMe(CancellationToken cancellationToken)
    {
        var query = new GetMeQuery();

        var result = await _sender.Send(query, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }

    [HttpGet("addresses")]
    [ProtectedResource(Resources.RESOURCE_USERS, Scopes.READ_OWN)]
    public async Task<IActionResult> GetAddresses(CancellationToken cancellationToken)
    {
        var query = new GetAddressesQuery();

        var result = await _sender.Send(query, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }


    [HttpPost("addresses")]
    [ProtectedResource(Resources.RESOURCE_USERS, Scopes.UPDATE_OWN)]
    [SwaggerHeader("X-Address-Type", "Type of address being added (home, work, billing, shipping)", "home", false)]
    public async Task<IActionResult> AddAddress([FromBody] AddAddressRequest request, CancellationToken cancellationToken)
    {
        // var addressType = Request.Headers["X-Address-Type"].FirstOrDefault() ?? "home";

        var cmd = _mapper.Map<AddAddressCommand>(request);

        var result = await _sender.Send(cmd, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }


    [HttpPut("addresses/{addressId}")]
    [ProtectedResource(Resources.RESOURCE_USERS, Scopes.UPDATE_OWN)]
    public async Task<IActionResult> UpdateAddress([FromRoute] string addressId, [FromBody] UpdateAddressRequest request, CancellationToken cancellationToken)
    {
        var cmd = _mapper.Map<UpdateAddressCommand>(request);
        cmd.AddressId = addressId;

        var result = await _sender.Send(cmd, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }

    [HttpPatch("addresses/{addressId}/is-default")]
    [ProtectedResource(Resources.RESOURCE_USERS, Scopes.UPDATE_OWN)]
    public async Task<IActionResult> SetDefaultAddress([FromRoute] string addressId, CancellationToken cancellationToken)
    {
        var cmd = new SetDefaultAddressCommand(addressId);

        var result = await _sender.Send(cmd, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }

    [HttpPut("{userId}/profiles")]
    [OpenApiOperation("Update profile by user id", "Accessible only to ADMIN or ADMIN_SUPER roles.")]
    public async Task<IActionResult> UpdateProfileById([FromRoute] string userId, [FromBody] UpdateProfileRequest request, CancellationToken cancellationToken)
    {
        var cmd = _mapper.Map<UpdateProfileByIdCommand>(request);
        cmd.UserId = userId;

        var result = await _sender.Send(cmd, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }

    [HttpPut("profiles")]
    [ProtectedResource(Resources.RESOURCE_USERS, Scopes.UPDATE_OWN)]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest request, CancellationToken cancellationToken)
    {
        var cmd = _mapper.Map<UpdateProfileCommand>(request);

        var result = await _sender.Send(cmd, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }

    [HttpDelete("addresses/{addressId}")]
    [ProtectedResource(Resources.RESOURCE_USERS, Scopes.DELETE_OWN)]
    public async Task<IActionResult> DeleteAddress([FromRoute] string addressId, CancellationToken cancellationToken)
    {
        var cmd = new DeleteAddressCommand(addressId);

        var result = await _sender.Send(cmd, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }
}



