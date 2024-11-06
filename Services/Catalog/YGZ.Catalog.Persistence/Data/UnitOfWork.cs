

using YGZ.Catalog.Domain.Core.Abstractions.Data;

namespace YGZ.Catalog.Persistence.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly IMongoContext _context;

    public UnitOfWork(IMongoContext context)
    {
        _context = context;
    }

    public async Task<bool> Commit()
    {
        var changeAmount = await _context.SaveChanges();

        return changeAmount > 0;
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}