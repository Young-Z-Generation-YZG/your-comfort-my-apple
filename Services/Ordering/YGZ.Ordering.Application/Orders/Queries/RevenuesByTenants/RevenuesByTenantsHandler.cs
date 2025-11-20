using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Ordering;
using YGZ.BuildingBlocks.Shared.Utils;
using YGZ.Ordering.Application.Abstractions.Data;
using YGZ.Ordering.Domain.Orders.ValueObjects;

namespace YGZ.Ordering.Application.Orders.Queries.RevenuesByTenants;

public class RevenuesByTenantsHandler : IQueryHandler<RevenuesByTenantsQuery, RevenuesByGroupResponse>
{
    private readonly ILogger<RevenuesByTenantsHandler> _logger;
    private readonly IGenericRepository<Order, OrderId> _repository;

    public RevenuesByTenantsHandler(ILogger<RevenuesByTenantsHandler> logger,
                                   IGenericRepository<Order, OrderId> repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task<Result<RevenuesByGroupResponse>> Handle(RevenuesByTenantsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // If no tenants provided, return empty dictionary
            if (request.Tenants == null || !request.Tenants.Any())
            {
                return new RevenuesByGroupResponse
                {
                    Groups = new Dictionary<string, List<OrderDetailsResponse>>()
                };
            }

            // Filter out null or empty tenant IDs
            var tenantList = request.Tenants
                .Where(t => !string.IsNullOrWhiteSpace(t))
                .ToList();

            if (!tenantList.Any())
            {
                return new RevenuesByGroupResponse
                {
                    Groups = new Dictionary<string, List<OrderDetailsResponse>>()
                };
            }

            // Build filter expression for the tenants and dates
            var filterExpression = BuildExpression(tenantList, request.StartDate, request.EndDate);

            var includeExpressions = new Expression<Func<Order, object>>[]
            {
                x => x.OrderItems
            };

            // Get all orders matching the tenants
            var orders = await _repository.GetAllAsync(
                filterExpression: filterExpression,
                orderBy: null,
                includeExpressions: includeExpressions,
                cancellationToken: cancellationToken);

            // Group orders by tenant ID
            var groupedOrders = orders
                .Select(order => new { Order = order, Tenant = order.TenantId?.Value })
                .Where(x => x.Tenant is not null && tenantList.Contains(x.Tenant))
                .GroupBy(x => x.Tenant!)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(x => x.Order.ToResponse()).ToList()
                );

            // Ensure all requested tenants are in the dictionary (even if empty)
            var responseData = new Dictionary<string, List<OrderDetailsResponse>>();
            foreach (var tenant in tenantList)
            {
                responseData[tenant] = groupedOrders.ContainsKey(tenant)
                    ? groupedOrders[tenant]
                    : new List<OrderDetailsResponse>();
            }

            return new RevenuesByGroupResponse
            {
                Groups = responseData
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving revenues by tenants: {ErrorMessage}", ex.Message);
            throw;
        }
    }

    private static Expression<Func<Order, bool>> BuildExpression(List<string> tenants, DateTime? startDate, DateTime? endDate)
    {
        var filterExpression = ExpressionBuilder.New<Order>();

        // Filter out deleted orders
        filterExpression = filterExpression.And(order => !order.IsDeleted);

        // Filter by StartDate (CreatedAt >= StartDate at 00:00:00 UTC)
        if (startDate.HasValue)
        {
            // Ensure UTC DateTime for PostgreSQL compatibility
            var startDateValue = startDate.Value.Date;
            var startDateUtc = DateTime.SpecifyKind(startDateValue, DateTimeKind.Utc);
            filterExpression = filterExpression.And(order => order.CreatedAt >= startDateUtc);
        }

        // Filter by EndDate (CreatedAt < EndDate + 1 day, which means <= EndDate at 23:59:59 UTC)
        if (endDate.HasValue)
        {
            // Ensure UTC DateTime for PostgreSQL compatibility
            var endDateValue = endDate.Value.Date.AddDays(1);
            var endDateUtc = DateTime.SpecifyKind(endDateValue, DateTimeKind.Utc);
            filterExpression = filterExpression.And(order => order.CreatedAt < endDateUtc);
        }

        return filterExpression;
    }
}
