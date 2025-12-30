using System.Linq.Expressions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Constants;
using YGZ.BuildingBlocks.Shared.Contracts.Common;
using YGZ.BuildingBlocks.Shared.Contracts.Identity;
using YGZ.BuildingBlocks.Shared.Utils;
using YGZ.Identity.Application.Abstractions.Data;
using YGZ.Identity.Domain.Users;

namespace YGZ.Identity.Application.Users.Queries.GetUsers;

public class GetUsersQueryHandler : IQueryHandler<GetUsersQuery, PaginationResponse<UserResponse>>
{
    private readonly ILogger<GetUsersQueryHandler> _logger;
    private readonly IIdentityDbContext _identityDbContext;
    private readonly DbSet<User> _userDbSet;

    public GetUsersQueryHandler(
        ILogger<GetUsersQueryHandler> logger,
        IIdentityDbContext identityDbContext)
    {
        _logger = logger;
        _identityDbContext = identityDbContext;
        _userDbSet = identityDbContext.Users;
    }

    public async Task<Result<PaginationResponse<UserResponse>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var dbContext = _identityDbContext.GetDbContext();
            var userRoles = dbContext.Set<IdentityUserRole<string>>();
            var roles = dbContext.Set<IdentityRole>();

            IQueryable<User> query = _userDbSet
                .AsNoTracking()
                .Include(x => x.Profile);

            query = query.IgnoreQueryFilters();

            // Filter only users with "USER" role
            query = query.Where(user => userRoles
                .Join(roles,
                    userRole => userRole.RoleId,
                    role => role.Id,
                    (userRole, role) => new { userRole.UserId, role.Name })
                .Where(x => x.UserId == user.Id && x.Name == AuthorizationConstants.Roles.USER)
                .Any());

            // Apply additional filters from request
            var filterExpression = BuildExpression(request);
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
                nameof(Handle), "Successfully retrieved users", new { totalRecords, totalPages, currentPage, pageSize });

            return response;
        }
        catch (Exception ex)
        {
            var parameters = new { page = request.Page, limit = request.Limit };
            _logger.LogError(ex, ":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(Handle), ex.Message, parameters);
            
            throw;
        }
    }

    private static Expression<Func<User, bool>> BuildExpression(GetUsersQuery request)
    {
        var filterExpression = ExpressionBuilder.New<User>();

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
        GetUsersQuery request)
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
