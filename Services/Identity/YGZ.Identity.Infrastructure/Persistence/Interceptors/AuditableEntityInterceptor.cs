
//using Microsoft.EntityFrameworkCore.Diagnostics;
//using Microsoft.EntityFrameworkCore;
//using YGZ.Identity.Domain.Core.Primitives;
//using YGZ.Identity.Domain.Core.Abstractions;

//namespace YGZ.Identity.Infrastructure.Persistence.Interceptors;

//public class AuditableEntityInterceptor : SaveChangesInterceptor
//{
//    public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
//    {
//        UpdateEntities(eventData.Context);
//        return base.SavedChanges(eventData, result);
//    }

//    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
//    {
//        UpdateEntities(eventData.Context);
//        return base.SavingChangesAsync(eventData, result, cancellationToken);
//    }

//    private void UpdateEntities(DbContext? context)
//    {
//        if (context == null) return;

//        foreach (var entry in context.ChangeTracker.Entries<IAuditable<ValueObject>>())
//        {
//            if (entry.State == EntityState.Added)
//            {
//                entry.Entity.UpdatedAt = DateTime.UtcNow;
//                entry.Entity.LastModifiedBy = null;
//            }

//            if (entry.State == EntityState.Modified)
//            {
//                entry.Entity.UpdatedAt = DateTime.UtcNow;
//                entry.Entity.LastModifiedBy = UserId.Create();
//            }
//        }
//    }
//}
