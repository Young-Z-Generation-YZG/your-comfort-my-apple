
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Identity.Domain.Users;
using YGZ.Identity.Domain.Users.Entities;

namespace YGZ.Identity.Application.Abstractions.Data;

public interface IUserRepository
{
    Task<Result<User>> GetUserByEmailAsync(string userEmail, CancellationToken cancellationToken);
    Task<Result<User>> GetUserByEmailAsync(string userEmail, CancellationToken cancellationToken, params Expression<Func<User, object>>[] expressions);
    Task<Result<bool>> AddShippingAddressAsync(ShippingAddress shippingAddress, User user, CancellationToken cancellationToken);
    Task<Result<bool>> UpdateUserAsync(User user, CancellationToken cancellationToken);
    DbSet<User> GetDbSet();
    Task SaveChange();
}
