

using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.HttpContext;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Identity.Application.Abstractions.Data;
using YGZ.Identity.Application.Users.Queries.GetProfile;
using YGZ.Identity.Domain.Users;
using YGZ.Identity.Domain.Users.Entities;
using YGZ.Identity.Domain.Users.ValueObjects;

namespace YGZ.Identity.Application.Users.Commands.AddAddress;

public class AddAddressHandler : ICommandHandler<AddAddressCommand, bool>
{
    private readonly IUserHttpContext _userHttpContext;
    private readonly ILogger<GetMeHandler> _logger;
    private readonly IUserRepository _userRepository;
    private readonly IGenericRepository<ShippingAddress, ShippingAddressId> _addressRepository;

    public AddAddressHandler(IUserHttpContext userHttpContext,
                             ILogger<GetMeHandler> logger,
                             IUserRepository userRepository,
                             IGenericRepository<ShippingAddress, ShippingAddressId> addressRepository)
    {
        _userHttpContext = userHttpContext;
        _logger = logger;
        _userRepository = userRepository;
        _addressRepository = addressRepository;
    }

    public async Task<Result<bool>> Handle(AddAddressCommand request, CancellationToken cancellationToken)
    {
        var userId = _userHttpContext.GetUserId();

        var expressions = new Expression<Func<User, object>>[]
        {
                x => x.ShippingAddresses
        };

        var result = await _userRepository.GetByIdAsync(userId, expressions, cancellationToken);

        if (result.IsFailure)
        {
            return result.Error;
        }

        var user = result.Response!;

        var newShippingAddress = ShippingAddress.Create(
            id: ShippingAddressId.Create(),
            label: request.Label,
            contactName: request.ContactName,
            contactPhoneNumber: request.ContactPhoneNumber,
            addressDetail: Address.Create(
                addressLine: request.AddressLine,
                addressDistrict: request.District,
                addressProvince: request.Province,
                addressCountry: request.Country),
            isDefault: false,
            userId: userId
        );



        var addResult = await _addressRepository.AddAsync(newShippingAddress, cancellationToken);

        if (addResult.IsFailure)
        {
            return addResult.Error;
        }

        return addResult.Response;
    }
}
