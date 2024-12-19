

using Microsoft.EntityFrameworkCore;
using YGZ.Ordering.Domain.Orders;
using YGZ.Ordering.Domain.Orders.Entities;

namespace YGZ.Ordering.Application.Core.Abstractions.Data;

public interface IApplicationDbContext
{
    DbSet<Customer> Customers { get; }
    DbSet<Product> Products { get; }
    DbSet<Order> Orders { get; }
    DbSet<OrderLine> OrderLines { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
