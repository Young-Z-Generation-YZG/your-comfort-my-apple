using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Identity;
using YGZ.Identity.Application.Abstractions.Data;
using YGZ.Identity.Domain.Core.Errors;
using YGZ.Identity.Domain.Users;

namespace YGZ.Identity.Application.Users.Queries.GetUserById;

public class GetUserByIdHandler : IQueryHandler<GetUserByIdQuery, UserResponse>
{
    private readonly ILogger<GetUserByIdHandler> _logger;
    private readonly IIdentityDbContext _identityDbContext;
    private readonly DbSet<User> _userDbSet;

    public GetUserByIdHandler(ILogger<GetUserByIdHandler> logger,
                              IIdentityDbContext identityDbContext)
    {
        _logger = logger;
        _identityDbContext = identityDbContext;
        _userDbSet = identityDbContext.Users;
    }

    public async Task<Result<UserResponse>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var query = _userDbSet
                .AsNoTracking()
                .Include(x => x.Profile)
                .IgnoreQueryFilters();

            var user = await query.FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken);

            if (user is null)
            {
                _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                    "FirstOrDefaultAsync", "User not found", new { request.UserId });

                return Errors.User.DoesNotExist;
            }

            if (user.Profile is null)
            {
                _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                    nameof(Handle), "Profile not found for user", new { request.UserId });

                return Errors.Profile.DoesNotExist;
            }

            _logger.LogInformation(":::[Handler Information]::: Method: {MethodName}, Information message: {InformationMessage}, Parameters: {@Parameters}",
                nameof(Handle), "Successfully retrieved user by ID", new { request.UserId });

            return user.ToResponse();
        }
        catch (Exception ex)
        {
            var parameters = new { userId = request.UserId };
            _logger.LogError(ex, ":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(Handle), ex.Message, parameters);

            throw;
        }
    }
}
