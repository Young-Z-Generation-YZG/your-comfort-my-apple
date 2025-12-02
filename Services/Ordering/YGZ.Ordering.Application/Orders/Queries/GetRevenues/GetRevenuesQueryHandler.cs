using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.HttpContext;
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
    private readonly ITenantHttpContext _tenantHttpContext;

    public GetRevenuesQueryHandler(ILogger<GetRevenuesQueryHandler> logger,
                                   IGenericRepository<Order, OrderId> repository,
                                   ITenantHttpContext tenantHttpContext)
    {
        _logger = logger;
        _repository = repository;
        _tenantHttpContext = tenantHttpContext;
    }

    public async Task<Result<List<OrderDetailsResponse>>> Handle(GetRevenuesQuery request, CancellationToken cancellationToken)
    {
        var tenantId = _tenantHttpContext.GetTenantId();
        
        var filterExpression = BuildExpression(request, tenantId);

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

    private static Expression<Func<Order, bool>> BuildExpression(GetRevenuesQuery request, string? tenantId)
    {
        var filterExpression = ExpressionBuilder.New<Order>();

        // Filter out deleted orders
        filterExpression = filterExpression.And(order => !order.IsDeleted);

        // Filter by tenantId (default filter)
        if (!string.IsNullOrWhiteSpace(tenantId))
        {
            var tenantIdValue = tenantId;
            // Filter by comparing TenantId.Value directly (handles null TenantId gracefully)
            // Note: Warnings are expected here due to nullable reference types in expression trees
            filterExpression = filterExpression.And(order => 
                order.TenantId != null && 
                order.TenantId.Value == tenantIdValue);
        }

        // Default date range: Jan 2025 to Dec 2025
        var defaultStartDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var defaultEndDate = new DateTime(2025, 12, 31, 23, 59, 59, DateTimeKind.Utc).AddDays(1); // Add 1 day to make it exclusive

        // Filter by StartDate (CreatedAt >= StartDate at 00:00:00 UTC)
        if (request.StartDate.HasValue)
        {
            // Ensure UTC DateTime for PostgreSQL compatibility
            var startDateValue = request.StartDate.Value.Date;
            var startDate = DateTime.SpecifyKind(startDateValue, DateTimeKind.Utc);
            filterExpression = filterExpression.And(order => order.CreatedAt >= startDate);
        }
        else
        {
            // Apply default start date (Jan 1, 2025)
            filterExpression = filterExpression.And(order => order.CreatedAt >= defaultStartDate);
        }

        // Filter by EndDate (CreatedAt < EndDate + 1 day, which means <= EndDate at 23:59:59 UTC)
        if (request.EndDate.HasValue)
        {
            // Ensure UTC DateTime for PostgreSQL compatibility
            var endDateValue = request.EndDate.Value.Date.AddDays(1);
            var endDate = DateTime.SpecifyKind(endDateValue, DateTimeKind.Utc);
            filterExpression = filterExpression.And(order => order.CreatedAt < endDate);
        }
        else
        {
            // Apply default end date (Dec 31, 2025)
            filterExpression = filterExpression.And(order => order.CreatedAt < defaultEndDate);
        }

        return filterExpression;
    }
}
