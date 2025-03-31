
using YGZ.Identity.Domain.Users.Entities;

namespace YGZ.Identity.Application.Abstractions.Data;

public interface IShippingAddressRepository
{
    Task<bool> AddAsync(ShippingAddress shippingAddress);
}
