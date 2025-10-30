
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Identity.Domain.Users;

namespace YGZ.Identity.Application.Abstractions.Data;

public interface IUserRepository
{
    Task<Result<User>> GetByIdAsync(string userId, Expression<Func<User, object>>[]? expressions = null, CancellationToken? cancellationToken = null);
    Task<Result<User>> GetUserByEmailAsync(string userEmail, Expression<Func<User, object>>[]? expressions = null, CancellationToken? cancellationToken = null);
    Task<Result<bool>> UpdateUserAsync(User user, CancellationToken? cancellationToken = null);
    DbSet<User> GetDbSet();
    Task SaveChange();
}
