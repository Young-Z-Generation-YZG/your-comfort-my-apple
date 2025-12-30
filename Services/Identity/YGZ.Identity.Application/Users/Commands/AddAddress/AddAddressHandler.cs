

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
    private readonly ILogger<AddAddressHandler> _logger;
    private readonly IUserRepository _userRepository;
    private readonly IGenericRepository<ShippingAddress, ShippingAddressId> _addressRepository;

    public AddAddressHandler(IUserHttpContext userHttpContext,
                             ILogger<AddAddressHandler> logger,
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
        try
        {
            var userId = _userHttpContext.GetUserId();

            var expressions = new Expression<Func<User, object>>[]
            {
                x => x.ShippingAddresses
            };

            var result = await _userRepository.GetByIdAsync(userId, expressions, cancellationToken);

            if (result.IsFailure)
            {
                _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                    nameof(_userRepository.GetByIdAsync), "User not found", new { userId, Error = result.Error });

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
                _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                    nameof(_addressRepository.AddAsync), "Failed to add address", new { userId, Error = addResult.Error });

                return addResult.Error;
            }

            _logger.LogInformation(":::[Handler Information]::: Method: {MethodName}, Information message: {InformationMessage}, Parameters: {@Parameters}",
                nameof(Handle), "Successfully added address", new { userId, addressId = newShippingAddress.Id });

            return addResult.Response;
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
