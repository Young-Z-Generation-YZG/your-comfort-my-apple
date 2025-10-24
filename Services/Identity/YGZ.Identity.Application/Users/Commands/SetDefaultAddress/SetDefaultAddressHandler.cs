

using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.HttpContext;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Identity.Application.Abstractions.Data;
using YGZ.Identity.Domain.Users.Entities;
using YGZ.Identity.Domain.Users.ValueObjects;

namespace YGZ.Identity.Application.Users.Commands.SetDefaultAddress;

public class SetDefaultAddressHandler : ICommandHandler<SetDefaultAddressCommand, bool>
{
    private readonly IUserHttpContext _userHttpContext;
    private readonly ILogger<SetDefaultAddressHandler> _logger;
    private readonly IUserRepository _userRepository;
    private readonly IGenericRepository<ShippingAddress, ShippingAddressId> _addressRepository;

    public SetDefaultAddressHandler(IUserHttpContext userHttpContext,
                                ILogger<SetDefaultAddressHandler> logger,
                                IUserRepository userRepository,
                                IGenericRepository<ShippingAddress, ShippingAddressId> addressRepository)
    {
        _userHttpContext = userHttpContext;
        _logger = logger;
        _userRepository = userRepository;
        _addressRepository = addressRepository;
    }

    public async Task<Result<bool>> Handle(SetDefaultAddressCommand request, CancellationToken cancellationToken)
    {
        var userId = _userHttpContext.GetUserId();

        var addressId = ShippingAddressId.Of(request.AddressId);

        var addressesResult = await _addressRepository.GetAllByFilterAsync(
            filterExpression: x => x.UserId == userId,
            cancellationToken: cancellationToken
        );

        var addresses = addressesResult.Response!;

        var addressResult = await _addressRepository.GetByIdAsync(addressId, cancellationToken: cancellationToken);

        if (addressResult.IsFailure)
        {
            return addressResult.Error;
        }

        var address = addressResult.Response!;

        foreach (var item in addresses)
        {
            item.IsDefault = false;
        }

        var targetAddress = addresses.FirstOrDefault(x => x.Id == address.Id);


        targetAddress!.IsDefault = true;

        var updateResult = await _addressRepository.UpdateRangeAsync(addresses, cancellationToken);

        if (updateResult.IsFailure)
        {
            return updateResult.Error;
        }

        return updateResult.Response;
    }
}
