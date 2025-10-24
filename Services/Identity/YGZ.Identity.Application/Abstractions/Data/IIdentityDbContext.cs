using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using YGZ.Identity.Domain.Users;
using YGZ.Identity.Domain.Users.Entities;

namespace YGZ.Identity.Application.Abstractions.Data;

public interface IIdentityDbContext
{
    DbSet<User> Users { get; }
    DbSet<Profile> Profiles { get; }
    DbSet<ShippingAddress> ShippingAddresses { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task<Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
}
