

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Identity.Application.Abstractions.Data;
using YGZ.Identity.Domain.Core.Errors;
using YGZ.Identity.Domain.Users;
using YGZ.Identity.Domain.Users.Entities;
using YGZ.Identity.Domain.Users.ValueObjects;

namespace YGZ.Identity.Infrastructure.Persistence.Repositories;

public class AddressRepository : GenericRepository<ShippingAddress, ShippingAddressId>, IAddressRepository
{
    private readonly IdentityDbContext _dbContext;
    private readonly ILogger<AddressRepository> _logger;
    public AddressRepository(IdentityDbContext context, ILogger<AddressRepository> logger) : base(context, logger)
    {
        _dbContext = context;
        _logger = logger;
    }

    public async Task<Result<List<ShippingAddress>>> GetAllByUser(User user)
    {
        try
        {
            var result = await _dbSet
                .Where(x => x.UserId == user.Id)
                .ToListAsync();

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message, $"class:{nameof(AddressRepository)} - method:{nameof(GetAllByUser)}");
            throw;
        }
    }

    public async Task<Result<bool>> SetDefaultAddressAsync(User user, ShippingAddress address, CancellationToken cancellationToken)
    {
        try
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

            // Load all addresses for the user
            var addresses = await _dbSet
                .Where(x => x.UserId == user.Id)
                .ToListAsync(cancellationToken);

            if (addresses.Count == 0)
            {
                return Errors.Address.NotFound;
            }

            // Find the address to update in the tracked list
            var targetAddress = addresses.FirstOrDefault(x => x.Id == address.Id);
            if (targetAddress is null)
            {
                return Errors.Address.NotFound;
            }

            // Update the default status
            foreach (var item in addresses)
            {
                item.IsDefault = false;
            }

            targetAddress.IsDefault = true;
            targetAddress.UpdatedAt = DateTime.UtcNow;

            // Update all addresses in one go
            _dbSet.UpdateRange(addresses);

            var result = await _dbContext.SaveChangesAsync(cancellationToken);
            if (result > 0)
            {
                await transaction.CommitAsync(cancellationToken);
                return true;
            }

            await transaction.RollbackAsync(cancellationToken);
            return Errors.Address.CanNotUpdate;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message, $"class:{nameof(AddressRepository)} - method:{nameof(SetDefaultAddressAsync)}");
            throw;
        }
    }
}
