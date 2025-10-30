
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
        var userId = _userHttpContext.GetUserId();

        var expressions = new Expression<Func<User, object>>[]
        {
                x => x.ShippingAddresses
        };

        var userResult = await _userRepository.GetByIdAsync(userId, expressions, cancellationToken);

        if (userResult.IsFailure)
        {
            return userResult.Error;
        }

        var user = userResult.Response!;

        var addresses = user.ShippingAddresses.ToList();

        return addresses.Select(x => x.ToResponse()).ToList();

    }
}
