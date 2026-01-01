using System.Linq.Expressions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.HttpContext;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Constants;
using YGZ.BuildingBlocks.Shared.Contracts.Common;
using YGZ.BuildingBlocks.Shared.Contracts.Identity;
using YGZ.BuildingBlocks.Shared.Utils;
using YGZ.Identity.Application.Abstractions.Data;
using YGZ.Identity.Domain.Users;

namespace YGZ.Identity.Application.Users.Queries.GetUsersByAdmin;

public class GetUsersByAdminHandler : IQueryHandler<GetUsersByAdminQuery, PaginationResponse<UserResponse>>
{
    private readonly ILogger<GetUsersByAdminHandler> _logger;
    private readonly IUserHttpContext _userHttpContext;
    private readonly ITenantHttpContext _tenantHttpContext;
    private readonly IIdentityDbContext _identityDbContext;
    private readonly DbSet<User> _userDbSet;

    public GetUsersByAdminHandler(IUserHttpContext userHttpContext,
                                ILogger<GetUsersByAdminHandler> logger,
                                ITenantHttpContext tenantHttpContext,
                                IIdentityDbContext identityDbContext)
    {
        _logger = logger;
        _userHttpContext = userHttpContext;
        _tenantHttpContext = tenantHttpContext;
        _identityDbContext = identityDbContext;
        _userDbSet = identityDbContext.Users;
    }

    public async Task<Result<PaginationResponse<UserResponse>>> Handle(GetUsersByAdminQuery request, CancellationToken cancellationToken)
    {
        var userRolesFromContext = _userHttpContext.GetUserRoles();
        var isAdminSuperYBZone = userRolesFromContext.Contains(AuthorizationConstants.Roles.ADMIN_SUPER_YBZONE);
        var isAdminYBZone = userRolesFromContext.Contains(AuthorizationConstants.Roles.ADMIN_YBZONE);
        var tenantIdFromHeader = request.TenantId;
        var tenantIdFromContext = _tenantHttpContext.GetTenantId();

        _logger.LogInformation(":::[Debug Information]::: Method: {MethodName}, Debug message: {InformationMessage}, Parameters: {@Parameters}",
            nameof(Handle), "Debug data", new { userRolesFromContext, isAdminSuperYBZone, isAdminYBZone, tenantIdFromHeader, tenantIdFromContext });
        _logger.LogTrace(":::[Debug Information]::: Method: {MethodName}, Debug message: {InformationMessage}, Parameters: {@Parameters}",
            nameof(Handle), "Debug data", new { userRolesFromContext, isAdminSuperYBZone, isAdminYBZone, tenantIdFromHeader, tenantIdFromContext });

        try
        {
            var dbContext = _identityDbContext.GetDbContext();
            var userRoles = dbContext.Set<IdentityUserRole<string>>();
            var roles = dbContext.Set<IdentityRole>();

            IQueryable<User> query = _userDbSet
                .AsNoTracking()
                .Include(x => x.Profile);

            Expression<Func<User, bool>>? filterExpression = null;

            if (isAdminSuperYBZone)
            {
                query = query.IgnoreQueryFilters();

                // Exclude users with "ADMIN_SUPER" and "USER" roles
                query = query.Where(user => !userRoles
                    .Join(roles,
                        userRole => userRole.RoleId,
                        role => role.Id,
                        (userRole, role) => new { userRole.UserId, role.Name })
                    .Where(x => x.UserId == user.Id && (x.Name == AuthorizationConstants.Roles.USER))
                    .Any());

                filterExpression = BuildExpression(request, tenantIdFromHeader ?? string.Empty);
            }
            else if (isAdminYBZone)
            {
                query = query.IgnoreQueryFilters();

                query = query.Where(user => userRoles
                    .Join(roles,
                        userRole => userRole.RoleId,
                        role => role.Id,
                        (userRole, role) => new { userRole.UserId, role.Name })
                    .Where(x => x.UserId == user.Id && (x.Name == AuthorizationConstants.Roles.ADMIN_SUPER_YBZONE || x.Name == AuthorizationConstants.Roles.ADMIN_YBZONE || x.Name == AuthorizationConstants.Roles.ADMIN_BRANCH || x.Name == AuthorizationConstants.Roles.STAFF_BRANCH))
                    .Any());

                filterExpression = BuildExpression(request, tenantId: null);
            }
            else
            {
                query = query.Where(user => userRoles
                    .Join(roles,
                        userRole => userRole.RoleId,
                        role => role.Id,
                        (userRole, role) => new { userRole.UserId, role.Name })
                    .Where(x => x.UserId == user.Id && (x.Name == AuthorizationConstants.Roles.ADMIN_BRANCH || x.Name == AuthorizationConstants.Roles.STAFF_BRANCH))
                    .Any());

                filterExpression = BuildExpression(request, tenantIdFromContext ?? string.Empty);
            }

            if (filterExpression != null)
            {
                query = query.Where(filterExpression);
            }

            // Apply sorting by CreatedAt DESC
            query = query.OrderByDescending(user => user.CreatedAt);

            // Calculate pagination
            var currentPage = request.Page ?? 1;
            var pageSize = request.Limit ?? 10;

            var totalRecords = await query.CountAsync(cancellationToken);
            var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

            // Apply pagination
            var users = await query
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            // Map to response
            var userResponses = users.Select(user => user.ToResponse()).ToList();

            var response = MapToResponse(userResponses, totalRecords, totalPages, request);

            _logger.LogInformation(":::[Handler Information]::: Method: {MethodName}, Information message: {InformationMessage}, Parameters: {@Parameters}",
                nameof(Handle), "Successfully retrieved users by admin", new { totalRecords, totalPages, currentPage, pageSize, isAdminSuperYBZone, isAdminYBZone });

            return response;
        }
        catch (Exception ex)
        {
            var parameters = new { page = request.Page, limit = request.Limit, tenantId = request.TenantId };
            _logger.LogError(ex, ":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(Handle), ex.Message, parameters);

            throw;
        }
    }

    private static Expression<Func<User, bool>> BuildExpression(GetUsersByAdminQuery request, string tenantId)
    {
        var filterExpression = ExpressionBuilder.New<User>();

        if (!string.IsNullOrWhiteSpace(tenantId))
        {
            filterExpression = filterExpression.And(user => user.TenantId == tenantId);
        }

        if (!string.IsNullOrWhiteSpace(request.Email))
        {
            filterExpression = filterExpression.And(user => user.Email != null && user.Email.Contains(request.Email));
        }

        if (!string.IsNullOrWhiteSpace(request.PhoneNumber))
        {
            filterExpression = filterExpression.And(user => user.PhoneNumber != null && user.PhoneNumber.Contains(request.PhoneNumber));
        }

        if (!string.IsNullOrWhiteSpace(request.FirstName))
        {
            filterExpression = filterExpression.And(user => user.Profile.FirstName.Contains(request.FirstName));
        }

        if (!string.IsNullOrWhiteSpace(request.LastName))
        {
            filterExpression = filterExpression.And(user => user.Profile.LastName.Contains(request.LastName));
        }

        return filterExpression;
    }

    private static PaginationResponse<UserResponse> MapToResponse(
        List<UserResponse> users,
        int totalRecords,
        int totalPages,
        GetUsersByAdminQuery request)
    {
        var queryParams = QueryParamBuilder.Build(request);

        var links = PaginationLinksBuilder.Build(
            basePath: "",
            queryParams: queryParams,
            currentPage: request.Page ?? 1,
            totalPages: totalPages);

        var response = new PaginationResponse<UserResponse>
        {
            TotalRecords = totalRecords,
            TotalPages = totalPages,
            PageSize = request.Limit ?? 10,
            CurrentPage = request.Page ?? 1,
            Items = users,
            Links = links
        };

        return response;
    }
}
