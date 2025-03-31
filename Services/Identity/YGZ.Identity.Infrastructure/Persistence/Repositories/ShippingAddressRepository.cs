

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using YGZ.Identity.Application.Abstractions.Data;
using YGZ.Identity.Domain.Users.Entities;

namespace YGZ.Identity.Infrastructure.Persistence.Repositories;

public class ShippingAddressRepository : IShippingAddressRepository
{
    private readonly IdentityDbContext _context;
    private readonly DbSet<ShippingAddress> _dbSet;
    private readonly ILogger<ShippingAddressRepository> _logger;
    public ShippingAddressRepository(IdentityDbContext context, ILogger<ShippingAddressRepository> logger)
    {
        _context = context;
        _dbSet = context.Set<ShippingAddress>();
        _logger = logger;
    }

    public async Task<bool> AddAsync(ShippingAddress shippingAddress)
    {
        try
        {
            await _context.ShippingAddresses.AddAsync(shippingAddress);

            await _context.SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message, nameof(AddAsync));

            return false;
        }
    }
}
