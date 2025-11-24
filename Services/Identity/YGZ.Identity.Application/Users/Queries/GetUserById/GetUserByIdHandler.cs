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
                return Errors.User.DoesNotExist;
            }

            if (user.Profile is null)
            {
                return Errors.Profile.DoesNotExist;
            }

            return user.ToResponse();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user by id: {UserId}", request.UserId);
            throw;
        }
    }
}
