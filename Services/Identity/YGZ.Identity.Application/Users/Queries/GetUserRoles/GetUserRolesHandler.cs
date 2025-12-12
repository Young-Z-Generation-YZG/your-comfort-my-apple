using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Identity.Domain.Core.Errors;
using YGZ.Identity.Domain.Users;

namespace YGZ.Identity.Application.Users.Queries.GetUserRoles;

public class GetUserRolesHandler : IQueryHandler<GetUserRolesQuery, List<string>>
{
    private readonly ILogger<GetUserRolesHandler> _logger;
    private readonly UserManager<User> _userManager;

    public GetUserRolesHandler(ILogger<GetUserRolesHandler> logger, UserManager<User> userManager)
    {
        _logger = logger;
        _userManager = userManager;
    }

    public async Task<Result<List<string>>> Handle(GetUserRolesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (!Guid.TryParse(request.UserId, out _))
            {
                return Errors.User.DoesNotExist;
            }

            // Ignore tenant filter when fetching the user by Id
            var user = await _userManager.Users
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

            if (user is null)
            {
                return Errors.User.DoesNotExist;
            }

            var roles = await _userManager.GetRolesAsync(user);
            return roles.ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting roles for user {UserId}", request.UserId);
            throw;
        }
    }
}


