using System.Linq.Expressions;
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
        _userHttpContext = userHttpContext;
        _logger = logger;
        _tenantHttpContext = tenantHttpContext;
        _identityDbContext = identityDbContext;
        _userDbSet = identityDbContext.Users;
    }

    public async Task<Result<PaginationResponse<UserResponse>>> Handle(GetUsersByAdminQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var filterExpression = BuildExpression(request);

            var currentPage = request.Page ?? 1;
            var pageSize = request.Limit ?? 10;

            var query = _userDbSet.AsNoTracking()
                .Where(filterExpression)
                .Include(x => x.Profile);

            var totalRecords = await query.CountAsync(cancellationToken);
            var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

            var users = await query
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            var userResponses = users.Select(user => user.ToResponse()).ToList();

            var response = MapToResponse(userResponses, totalRecords, totalPages, request);

            return response;
        }
        catch (Exception)
        {
            throw;
        }
    }

    private static Expression<Func<User, bool>> BuildExpression(GetUsersByAdminQuery request)
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
