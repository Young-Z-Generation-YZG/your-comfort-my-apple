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
    private readonly IUserRepository _repository;
    private readonly IIdentityService _identityService;

    public GetIdentityHandler(ILogger<GetIdentityHandler> logger,
                        IUserHttpContext userHttpContext,
                        IUserRepository repository,
                        IIdentityService identityService)
    {
        _logger = logger;
        _userHttpContext = userHttpContext;
        _repository = repository;
        _identityService = identityService;
    }

    public async Task<Result<GetIdentityResponse>> Handle(GetIdentityQuery request, CancellationToken cancellationToken)
    {
        var userEmail = _userHttpContext.GetUserEmail();
        // Use FindUserAsyncIgnoreFilters to bypass tenant filtering for /me endpoint
        var userResult = await _identityService.FindUserAsyncIgnoreFilters(userEmail);

        if (userResult.IsFailure)
        {
            return userResult.Error;
        }

        var user = userResult.Response!;

        var rolesResult = await _identityService.GetRolesAsync(user);

        if (rolesResult.IsFailure)
        {
            return rolesResult.Error;
        }

        var response = new GetIdentityResponse
        {
            Id = user.Id,
            TenantId = user.TenantId,
            BranchId = user.BranchId,
            TenantSubDomain = null,
            Username = user.UserName ?? string.Empty,
            Email = user.Email!,
            EmailConfirmed = user.EmailConfirmed,
            PhoneNumber = user.PhoneNumber ?? string.Empty,
            Roles = rolesResult.Response!
        };

        return response;
    }
}
