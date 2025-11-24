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
        var userId = _userHttpContext.GetUserId();

        var includeExpressions = new Expression<Func<User, object>>[]
        {
                x => x.Profile
        };

        var userResult = await _repository.GetByIdAsync(userId, expressions: includeExpressions, cancellationToken: cancellationToken);

        if (userResult.IsFailure)
        {
            return userResult.Error;
        }

        return userResult.Response!.ToResponse();
    }
}
