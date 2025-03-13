

using YGZ.Ordering.Application.Orders;
using YGZ.Ordering.Domain.Orders.ValueObjects;

namespace YGZ.Ordering.Application.Abstractions.Data;

public interface IOrderRepository : IGenericRepository<Order,OrderId>
{
    Task<Order> GetOrderByCodeAsync(string code, CancellationToken cancellationToken);
}
