

using Microsoft.EntityFrameworkCore;
using YGZ.Ordering.Application.Orders;

namespace YGZ.Ordering.Infrastructure.Persistence;

public class OrderDbContext : DbContext
{
    public DbSet<Order> Orders { get; set; }
    public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
