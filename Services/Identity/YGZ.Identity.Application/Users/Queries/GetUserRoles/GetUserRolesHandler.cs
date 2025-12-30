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
                _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                    nameof(Guid.TryParse), "Invalid user ID format", new { request.UserId });

                return Errors.User.DoesNotExist;
            }

            // Ignore tenant filter when fetching the user by Id
            var user = await _userManager.Users
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

            if (user is null)
            {
                _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                    "FirstOrDefaultAsync", "User not found", new { request.UserId });

                return Errors.User.DoesNotExist;
            }

            var roles = await _userManager.GetRolesAsync(user);
            
            _logger.LogInformation(":::[Handler Information]::: Method: {MethodName}, Information message: {InformationMessage}, Parameters: {@Parameters}",
                nameof(Handle), "Successfully retrieved user roles", new { request.UserId, roleCount = roles.Count });

            return roles.ToList();
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


