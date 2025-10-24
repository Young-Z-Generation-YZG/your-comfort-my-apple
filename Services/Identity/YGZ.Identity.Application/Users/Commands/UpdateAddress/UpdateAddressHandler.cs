

using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.HttpContext;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Identity.Application.Abstractions.Data;
using YGZ.Identity.Domain.Users.Entities;
using YGZ.Identity.Domain.Users.ValueObjects;

namespace YGZ.Identity.Application.Users.Commands.UpdateAddress;

public class UpdateAddressHandler : ICommandHandler<UpdateAddressCommand, bool>
{
    private readonly IUserHttpContext _userHttpContext;
    private readonly ILogger<UpdateAddressHandler> _logger;
    private readonly IGenericRepository<ShippingAddress, ShippingAddressId> _repository;

    public UpdateAddressHandler(IUserHttpContext userHttpContext,
                             ILogger<UpdateAddressHandler> logger,
                             IGenericRepository<ShippingAddress, ShippingAddressId> repository)
    {
        _userHttpContext = userHttpContext;
        _logger = logger;
        _repository = repository;
    }

    public async Task<Result<bool>> Handle(UpdateAddressCommand request, CancellationToken cancellationToken)
    {
        var userId = _userHttpContext.GetUserId();

        var addressId = ShippingAddressId.Of(request.AddressId);

        var addressResult = await _repository.GetByIdAsync(addressId, cancellationToken: cancellationToken);

        if (addressResult.IsFailure)
        {
            return addressResult.Error;
        }

        var address = addressResult.Response!;

        address.Update(
           label: request.Label,
           contactName: request.ContactName,
           contactPhoneNumber: request.ContactPhoneNumber,
           addressDetail: new Address
           {
               AddressLine = request.AddressLine,
               AddressDistrict = request.District,
               AddressProvince = request.Province,
               AddressCountry = request.Country
           },
           isDefault: false
        );

        var updateResult = await _repository.UpdateAsync(address, cancellationToken);

        if (updateResult.IsFailure)
        {
            return updateResult.Error;
        }

        return updateResult.Response;
    }
}
