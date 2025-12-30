
using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.HttpContext;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Identity;
using YGZ.Identity.Application.Abstractions.Data;
using YGZ.Identity.Domain.Users;

namespace YGZ.Identity.Application.Users.Queries.GetAddresses;

public class GetAddressesHandler : IQueryHandler<GetAddressesQuery, List<AddressResponse>>
{
    private readonly IUserHttpContext _userHttpContext;
    private readonly ILogger<GetAddressesHandler> _logger;
    private readonly IUserRepository _userRepository;

    public GetAddressesHandler(IUserHttpContext userHttpContext,
                                ILogger<GetAddressesHandler> logger,
                                IUserRepository userRepository)
    {
        _userHttpContext = userHttpContext;
        _logger = logger;
        _userRepository = userRepository;
    }

    public async Task<Result<List<AddressResponse>>> Handle(GetAddressesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var userId = _userHttpContext.GetUserId();

            var expressions = new Expression<Func<User, object>>[]
            {
                x => x.ShippingAddresses
            };

            var userResult = await _userRepository.GetByIdAsync(userId, expressions, cancellationToken);

            if (userResult.IsFailure)
            {
                _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                    nameof(_userRepository.GetByIdAsync), "User not found", new { userId, Error = userResult.Error });

                return userResult.Error;
            }

            var user = userResult.Response!;
            var addresses = user.ShippingAddresses.ToList();

            _logger.LogInformation(":::[Handler Information]::: Method: {MethodName}, Information message: {InformationMessage}, Parameters: {@Parameters}",
                nameof(Handle), "Successfully retrieved user addresses", new { userId, addressCount = addresses.Count });

            return addresses.Select(x => x.ToResponse()).ToList();
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
