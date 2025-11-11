using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.HttpContext;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Identity;
using YGZ.Identity.Application.Abstractions.Data;
using YGZ.Identity.Domain.Users;

namespace YGZ.Identity.Application.Users.Queries.GetListUsers;

public class GetListUsersHandler : IQueryHandler<GetListUsersQuery, List<UserResponse>>
{
    private readonly ILogger<GetListUsersHandler> _logger;
    private readonly IIdentityDbContext _identityDbContext;
    private readonly DbSet<User> _userDbSet;

    public GetListUsersHandler(ILogger<GetListUsersHandler> logger,
                               IIdentityDbContext identityDbContext)
    {
        _logger = logger;
        _identityDbContext = identityDbContext;
        _userDbSet = identityDbContext.Users;
    }

    public async Task<Result<List<UserResponse>>> Handle(GetListUsersQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Default roles to filter if not provided
            var rolesToFilter = request.Roles ?? new List<string> { "ADMIN", "STAFF" };

            // Get the underlying DbContext to access UserRoles and Roles using Set<>
            var dbContext = _identityDbContext.GetDbContext();
            var userRoles = dbContext.Set<IdentityUserRole<string>>();
            var roles = dbContext.Set<IdentityRole>();

            // Query users that have any of the specified roles
            var users = await _userDbSet
                .AsNoTracking()
                .Include(x => x.Profile)
                .Where(user => userRoles
                    .Join(roles,
                        userRole => userRole.RoleId,
                        role => role.Id,
                        (userRole, role) => new { userRole.UserId, role.Name })
                    .Where(x => x.UserId == user.Id && x.Name != null && rolesToFilter.Contains(x.Name))
                    .Any())
                .ToListAsync(cancellationToken);

            var userResponses = users.Select(user => user.ToResponse()).ToList();

            return userResponses;
        }
        catch (Exception)
        {
            throw;
        }
    }
}
