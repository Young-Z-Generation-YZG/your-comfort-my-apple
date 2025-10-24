using Microsoft.EntityFrameworkCore;
using YGZ.Identity.Application.Abstractions.Data;
using YGZ.Identity.Domain.Users;
using YGZ.Identity.Domain.Users.Entities;

namespace YGZ.Identity.Infrastructure.Persistence;

public class IdentityDbContextImpl : IIdentityDbContext
{
    private readonly IdentityDbContext _dbContext;

    public IdentityDbContextImpl(IdentityDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public DbSet<User> Users => _dbContext.Users;
    public DbSet<Profile> Profiles => _dbContext.Profiles;
    public DbSet<ShippingAddress> ShippingAddresses => _dbContext.ShippingAddresses;

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.Database.BeginTransactionAsync(cancellationToken);
    }
}
