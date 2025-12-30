using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.HttpContext;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Auth;
using YGZ.Identity.Application.Abstractions.Data;
using YGZ.Identity.Application.Abstractions.Services;

namespace YGZ.Identity.Application.Auths.Queries.GetIdentity;

public class GetIdentityHandler : IQueryHandler<GetIdentityQuery, GetIdentityResponse>
{
    private readonly ILogger<GetIdentityHandler> _logger;
    private readonly IUserHttpContext _userHttpContext;
    private readonly IIdentityService _identityService;
    private readonly ITenantHttpContext _tenantHttpContext;

    public GetIdentityHandler(ILogger<GetIdentityHandler> logger,
                        IUserHttpContext userHttpContext,
                        IIdentityService identityService,
                        ITenantHttpContext tenantHttpContext)
    {
        _logger = logger;
        _userHttpContext = userHttpContext;
        _identityService = identityService;
        _tenantHttpContext = tenantHttpContext;
    }

    public async Task<Result<GetIdentityResponse>> Handle(GetIdentityQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var userEmail = _userHttpContext.GetUserEmail();
            var tenantId = _tenantHttpContext.GetTenantId();
            var branchId = _tenantHttpContext.GetBranchId();

            var userResult = await _identityService.FindUserAsync(userEmail, ignoreBaseFilter: true);

            if (userResult.IsFailure)
            {
                _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                    nameof(_identityService.FindUserAsync), "User not found", new { userEmail });

                return userResult.Error;
            }

            var user = userResult.Response!;

            var rolesResult = await _identityService.GetRolesAsync(user);

            if (rolesResult.IsFailure)
            {
                _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                    nameof(_identityService.GetRolesAsync), "Failed to get user roles", rolesResult.Error);

                return rolesResult.Error;
            }

            var response = new GetIdentityResponse
            {
                Id = user.Id,
                TenantId = tenantId ?? user.TenantId,
                BranchId = branchId ?? user.BranchId,
                TenantSubDomain = "Not Available",
                Username = user.UserName ?? string.Empty,
                Email = user.Email!,
                EmailConfirmed = user.EmailConfirmed,
                PhoneNumber = user.PhoneNumber ?? string.Empty,
                Roles = rolesResult.Response!
            };

            _logger.LogInformation(":::[Handler Information]::: Method: {MethodName}, Information message: {InformationMessage}, Parameters: {@Parameters}",
                nameof(Handle), "Successfully retrieved user identity", new { userEmail, userId = user.Id });

            return response;
        }
        catch (Exception ex)
        {
            var userEmail = _userHttpContext.GetUserEmail();
            var parameters = new { userEmail };
            _logger.LogError(ex, ":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(Handle), ex.Message, parameters);
            
            throw;
        }
    }
}
