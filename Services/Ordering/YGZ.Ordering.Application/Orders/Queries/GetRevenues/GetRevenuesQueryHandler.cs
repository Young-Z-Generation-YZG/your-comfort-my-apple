using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Ordering;
using YGZ.BuildingBlocks.Shared.Utils;
using YGZ.Ordering.Application.Abstractions.Data;
using YGZ.Ordering.Application.Orders.Queries.RevenuesQuery;
using YGZ.Ordering.Domain.Orders.ValueObjects;

namespace YGZ.Ordering.Application.Orders.Queries.GetRevenues;

public class GetRevenuesQueryHandler : IQueryHandler<GetRevenuesQuery, List<OrderDetailsResponse>>
{
    private readonly ILogger<GetRevenuesQueryHandler> _logger;
    private readonly IGenericRepository<Order, OrderId> _repository;

    public GetRevenuesQueryHandler(ILogger<GetRevenuesQueryHandler> logger,
                                   IGenericRepository<Order, OrderId> repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task<Result<List<OrderDetailsResponse>>> Handle(GetRevenuesQuery request, CancellationToken cancellationToken)
    {

        var filterExpression = BuildExpression(request);

        var includeExpressions = new Expression<Func<Order, object>>[]
        {
            x => x.OrderItems
        };

        var orders = await _repository.GetAllAsync(
            filterExpression: filterExpression,
            includeExpressions: includeExpressions,
            orderBy: null,
            cancellationToken: cancellationToken);

        return orders.Select(order => order.ToResponse()).ToList();
    }

    private static Expression<Func<Order, bool>> BuildExpression(GetRevenuesQuery request)
    {
        var filterExpression = ExpressionBuilder.New<Order>();

        // Filter out deleted orders
        filterExpression = filterExpression.And(order => !order.IsDeleted);

        // Filter by StartDate (CreatedAt >= StartDate at 00:00:00 UTC)
        if (request.StartDate.HasValue)
        {
            // Ensure UTC DateTime for PostgreSQL compatibility
            var startDateValue = request.StartDate.Value.Date;
            var startDate = DateTime.SpecifyKind(startDateValue, DateTimeKind.Utc);
            filterExpression = filterExpression.And(order => order.CreatedAt >= startDate);
        }

        // Filter by EndDate (CreatedAt < EndDate + 1 day, which means <= EndDate at 23:59:59 UTC)
        if (request.EndDate.HasValue)
        {
            // Ensure UTC DateTime for PostgreSQL compatibility
            var endDateValue = request.EndDate.Value.Date.AddDays(1);
            var endDate = DateTime.SpecifyKind(endDateValue, DateTimeKind.Utc);
            filterExpression = filterExpression.And(order => order.CreatedAt < endDate);
        }

        return filterExpression;
    }
}
