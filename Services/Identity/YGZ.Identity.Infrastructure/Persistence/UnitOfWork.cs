
//using System.Data;
//using Microsoft.EntityFrameworkCore.Storage;
//using YGZ.Identity.Application.Abstractions.Data;

//namespace YGZ.Identity.Infrastructure.Persistence;

//public class UnitOfWork : IUnitOfWork
//{
//    private readonly IdentityDbContext _dbContext;

//    public UnitOfWork(IdentityDbContext dbContext)
//    {
//        _dbContext = dbContext;
//    }

//    public IDbTransaction BeginTransaction()
//    {
//        var transaction = _dbContext.Database.BeginTransaction();

//        return transaction.GetDbTransaction();  
//    }

//    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
//    {
//        return _dbContext.SaveChangesAsync(cancellationToken);
//    }
//}
