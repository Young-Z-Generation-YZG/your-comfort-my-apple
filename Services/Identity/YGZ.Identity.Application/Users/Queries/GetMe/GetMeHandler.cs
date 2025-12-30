using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.HttpContext;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Identity;
using YGZ.Identity.Application.Abstractions.Data;
using YGZ.Identity.Domain.Users;

namespace YGZ.Identity.Application.Users.Queries.GetProfile;

public class GetMeHandler : IQueryHandler<GetMeQuery, GetAccountResponse>
{
    private readonly ILogger<GetMeHandler> _logger;
    private readonly IUserHttpContext _userHttpContext;
    private readonly IUserRepository _repository;

    public GetMeHandler(ILogger<GetMeHandler> logger,
                        IUserHttpContext userHttpContext,
                        IUserRepository repository)
    {
        _logger = logger;
        _userHttpContext = userHttpContext;
        _repository = repository;
    }

    public async Task<Result<GetAccountResponse>> Handle(GetMeQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var userId = _userHttpContext.GetUserId();

            var expressions = new Expression<Func<User, object>>[]
            {
                x => x.Profile,
                x => x.ShippingAddresses.Where(sa => sa.IsDefault == true),
            };

            var user = await _repository.GetByIdAsync(userId, expressions: expressions, cancellationToken: cancellationToken);

            if (user.IsFailure)
            {
                _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                    nameof(_repository.GetByIdAsync), "User not found", new { userId, Error = user.Error });

                return user.Error;
            }

            _logger.LogInformation(":::[Handler Information]::: Method: {MethodName}, Information message: {InformationMessage}, Parameters: {@Parameters}",
                nameof(Handle), "Successfully retrieved user profile", new { userId });

            return user.Response!.ToAccountResponse();
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
