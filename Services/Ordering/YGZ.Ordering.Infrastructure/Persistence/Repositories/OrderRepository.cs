

using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using YGZ.Ordering.Application.Abstractions.Data;
using YGZ.Ordering.Application.Orders;
using YGZ.Ordering.Domain.Core.Primitives;
using YGZ.Ordering.Domain.Orders.ValueObjects;

namespace YGZ.Ordering.Infrastructure.Persistence.Repositories;

public class OrderRepository : GenericRepository<Order, OrderId>, IOrderRepository
{
    private readonly OrderDbContext _orderDbContext;
    public OrderRepository(OrderDbContext orderDbContext) : base(orderDbContext)
    {
        _orderDbContext = orderDbContext;
    }

    public async Task<Order> GetOrderByCodeAsync(string code, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _dbSet.AsTracking().Where(x => x.Code == Code.Of(code)).SingleOrDefaultAsync();

            return result ?? null!;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public override Task<Order> GetByIdAsync(OrderId id, CancellationToken cancellationToken)
    {
        return base.GetByIdAsync(id, cancellationToken);
    }

    public async Task<Order> GetOrderByIdWithInclude(OrderId id, Expression<Func<Order, object>> expression, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _orderDbContext.Orders.Include(expression).FirstOrDefaultAsync(o => o.Id == id, cancellationToken);

            return result ?? null!;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}
