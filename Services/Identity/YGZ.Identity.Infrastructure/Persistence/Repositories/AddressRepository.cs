

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Identity.Application.Abstractions.Data;
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
}
