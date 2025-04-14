
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Identity.Domain.Users;
using YGZ.Identity.Domain.Users.Entities;
using YGZ.Identity.Domain.Users.ValueObjects;

namespace YGZ.Identity.Application.Abstractions.Data;

public interface IAddressRepository : IGenericRepository<ShippingAddress, ShippingAddressId>
{
    Task<Result<List<ShippingAddress>>> GetAllByUser(User user);
    Task<Result<bool>> SetDefaultAddressAsync(User user, ShippingAddress address, CancellationToken cancellationToken);
}
