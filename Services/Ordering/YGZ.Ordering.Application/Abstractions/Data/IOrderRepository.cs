

using System.Linq.Expressions;
using YGZ.BuildingBlocks.Shared.ValueObjects;
using YGZ.Ordering.Application.Orders;
using YGZ.Ordering.Domain.Orders.ValueObjects;

namespace YGZ.Ordering.Application.Abstractions.Data;

public interface IOrderRepository : IGenericRepository<Order, OrderId>
{
    Task<Order> GetOrderByCodeAsync(string code, CancellationToken cancellationToken);
    Task<List<Order>> GetUserOrdersWithItemAsync(UserId userId, CancellationToken cancellationToken);
    Task<Order> GetOrderByIdWithInclude(OrderId id, Expression<Func<Order, object>> expression, CancellationToken cancellationToken);
}
