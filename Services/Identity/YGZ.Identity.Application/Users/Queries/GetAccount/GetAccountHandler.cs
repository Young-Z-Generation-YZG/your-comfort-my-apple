using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.HttpContext;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Identity;
using YGZ.Identity.Application.Abstractions.Data;
using YGZ.Identity.Domain.Users;

namespace YGZ.Identity.Application.Users.Queries.GetAccount;

public class GetAccountHandler : IQueryHandler<GetAccountQuery, UserResponse>
{
    private readonly ILogger<GetAccountHandler> _logger;
    private readonly IUserHttpContext _userHttpContext;
    private readonly IUserRepository _repository;

    public GetAccountHandler(ILogger<GetAccountHandler> logger,
                             IUserHttpContext userHttpContext,
                             IUserRepository repository)
    {
        _logger = logger;
        _repository = repository;
        _userHttpContext = userHttpContext;
    }

    public async Task<Result<UserResponse>> Handle(GetAccountQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var userId = _userHttpContext.GetUserId();

            var includeExpressions = new Expression<Func<User, object>>[]
            {
                x => x.Profile
            };

            var userResult = await _repository.GetByIdAsync(userId, expressions: includeExpressions, cancellationToken: cancellationToken);

            if (userResult.IsFailure)
            {
                _logger.LogError(":::[GetAccountHandler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                    nameof(_repository.GetByIdAsync), "User not found", new { userId, Error = userResult.Error });

                return userResult.Error;
            }

            _logger.LogInformation(":::[GetAccountHandler Information]::: Method: {MethodName}, Information message: {InformationMessage}, Parameters: {@Parameters}",
                nameof(Handle), "Successfully retrieved user account", new { userId });

            return userResult.Response!.ToResponse();
        }
        catch (Exception ex)
        {
            var userId = _userHttpContext.GetUserId();
            var parameters = new { userId };
            _logger.LogError(ex, ":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(Handle), ex.Message, parameters);
            
            throw;
        }
    }
}
