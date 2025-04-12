
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Identity.Domain.Users;
using YGZ.Identity.Domain.Users.Entities;

namespace YGZ.Identity.Application.Abstractions.Data;

public interface IUserRepository
{
    Task<Result<User>> GetUserByEmail(string userEmail);
    Task<Result<User>> GetUserByEmail(string userEmail, params Expression<Func<User, object>>[] expressions);
    Task<Result<bool>> AddShippingAddressAsync(ShippingAddress shippingAddress, User user);
    DbSet<User> GetDbSet();
    Task SaveChange();
}
