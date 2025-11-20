using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.HttpContext;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Constants;
using YGZ.BuildingBlocks.Shared.Contracts.Identity;
using YGZ.Identity.Application.Abstractions.Data;
using YGZ.Identity.Domain.Users;

namespace YGZ.Identity.Application.Users.Queries.GetListUsers;

public class GetListUsersHandler : IQueryHandler<GetListUsersQuery, List<UserResponse>>
{
    private readonly ILogger<GetListUsersHandler> _logger;
    private readonly IIdentityDbContext _identityDbContext;
    private readonly DbSet<User> _userDbSet;
    private readonly ITenantHttpContext _tenantHttpContext;
    private readonly IUserHttpContext _userHttpContext;

    public GetListUsersHandler(ILogger<GetListUsersHandler> logger,
                               IIdentityDbContext identityDbContext,
                               ITenantHttpContext tenantHttpContext,
                               IUserHttpContext userHttpContext)
    {
        _logger = logger;
        _identityDbContext = identityDbContext;
        _userDbSet = identityDbContext.Users;
        _tenantHttpContext = tenantHttpContext;
        _userHttpContext = userHttpContext;
    }

    public async Task<Result<List<UserResponse>>> Handle(GetListUsersQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var userRolesFromContext = _userHttpContext.GetUserRoles();
            var isAdminSuper = userRolesFromContext.Contains(AuthorizationConstants.Roles.ADMIN_SUPER);

            // Get the underlying DbContext to access UserRoles and Roles using Set<>
            var dbContext = _identityDbContext.GetDbContext();
            var userRoles = dbContext.Set<IdentityUserRole<string>>();
            var roles = dbContext.Set<IdentityRole>();

            IQueryable<User> query = _userDbSet
                .AsNoTracking()
                .Include(x => x.Profile);

            // If user is ADMIN_SUPER, bypass tenant query filters but exclude users with "USER" role
            if (isAdminSuper)
            {
                query = query.IgnoreQueryFilters();

                // Exclude users with "USER" role
                query = query.Where(user => !userRoles
                    .Join(roles,
                        userRole => userRole.RoleId,
                        role => role.Id,
                        (userRole, role) => new { userRole.UserId, role.Name })
                    .Where(x => x.UserId == user.Id && x.Name == AuthorizationConstants.Roles.USER)
                    .Any());
            }
            else
            {
                // Default roles to filter if not provided
                var rolesToFilter = request.Roles ?? null;

                if (rolesToFilter != null && rolesToFilter.Any())
                {
                    // Query users that have any of the specified roles
                    query = query.Where(user => userRoles
                        .Join(roles,
                            userRole => userRole.RoleId,
                            role => role.Id,
                            (userRole, role) => new { userRole.UserId, role.Name })
                        .Where(x => x.UserId == user.Id && x.Name != null && rolesToFilter.Contains(x.Name))
                        .Any());
                }
            }

            var users = await query.ToListAsync(cancellationToken);

            var userResponses = users.Select(user => user.ToResponse()).ToList();

            return userResponses;
        }
        catch (Exception)
        {
            throw;
        }
    }
}
