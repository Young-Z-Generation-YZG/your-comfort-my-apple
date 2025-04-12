
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Identity.Domain.Users;
using YGZ.Identity.Domain.Users.Entities;

namespace YGZ.Identity.Application.Abstractions.Data;

public interface IAddressRepository
{
    Task<Result<List<ShippingAddress>>> GetAllByUser(User user);
}
