using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Identity;
using YGZ.Identity.Application.Abstractions.Data;
using YGZ.Identity.Domain.Users;
using YGZ.Identity.Domain.Core.Errors;

namespace YGZ.Identity.Application.Users.Queries.GetUserById;

public class GetUserByIdHandler : IQueryHandler<GetUserByIdQuery, UserResponse>
{
    private readonly ILogger<GetUserByIdHandler> _logger;
    private readonly IUserRepository _repository;

    public GetUserByIdHandler(ILogger<GetUserByIdHandler> logger,
                              IUserRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task<Result<UserResponse>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var expressions = new Expression<Func<User, object>>[]
        {
            x => x.Profile
        };

        var userResult = await _repository.GetByIdAsync(request.UserId, expressions: expressions, cancellationToken: cancellationToken);

        if (userResult.IsFailure && userResult.Error == Errors.User.DoesNotExist)
        {
            return Errors.User.DoesNotExist;
        }

        return userResult.Response!.ToResponse();
    }
}
