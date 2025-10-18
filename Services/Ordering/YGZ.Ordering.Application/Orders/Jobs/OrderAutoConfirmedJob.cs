using Microsoft.Extensions.Logging;
using Quartz;
using YGZ.Ordering.Application.Abstractions.Data;
using YGZ.Ordering.Domain.Orders.ValueObjects;

namespace YGZ.Ordering.Application.Orders.Jobs;

public class OrderAutoConfirmedJob : IJob
{
    private readonly IGenericRepository<Order, OrderId> _repository;
    private readonly ILogger<OrderAutoConfirmedJob> _logger;

    public OrderAutoConfirmedJob(IGenericRepository<Order, OrderId> repository,
                                 ILogger<OrderAutoConfirmedJob> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {

        //JobDataMap jobData = context.MergedJobDataMap;

        //string? orderId = jobData.GetString("OrderId");

        //var order = await _orderRepository.GetOrderByIdWithInclude(OrderId.Of(orderId!), (o => o.OrderItems), context.CancellationToken);

        //if (order is null)
        //{
        //    throw new InvalidOperationException(
        //        $"Order with id {orderId} not found.");
        //}

        //// Check if the order is already confirmed
        //if (order.Status == OrderStatus.CONFIRMED)
        //{
        //    return;
        //}

        //// Check if the order is in a state that can be auto-confirmed
        //if (order.Status != OrderStatus.PENDING)
        //{
        //    throw new InvalidOperationException(
        //        $"Order with id {orderId} cannot be auto-confirmed because it is in status {order.Status}.");
        //}

        //// Confirm the order
        //order.Status = OrderStatus.CONFIRMED;

        //// Save the changes to the database
        //await _orderRepository.UpdateAsync(order, context.CancellationToken);

        //_logger.LogInformation($"Order with id {orderId} has been auto-confirmed.");
    }
}
