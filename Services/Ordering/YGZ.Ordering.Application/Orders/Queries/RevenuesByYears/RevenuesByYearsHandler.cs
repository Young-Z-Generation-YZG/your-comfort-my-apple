using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Ordering;
using YGZ.BuildingBlocks.Shared.Utils;
using YGZ.Ordering.Application.Abstractions.Data;
using YGZ.Ordering.Domain.Orders.ValueObjects;

namespace YGZ.Ordering.Application.Orders.Queries.RevenuesByYears;

public class RevenuesByYearsHandler : IQueryHandler<RevenuesByYearsQuery, RevenuesByGroupResponse>
{
    private readonly ILogger<RevenuesByYearsHandler> _logger;
    private readonly IGenericRepository<Order, OrderId> _repository;

    public RevenuesByYearsHandler(ILogger<RevenuesByYearsHandler> logger,
                                   IGenericRepository<Order, OrderId> repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task<Result<RevenuesByGroupResponse>> Handle(RevenuesByYearsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // If no years provided, return empty dictionary
            if (request.Years == null || !request.Years.Any())
            {
                return new RevenuesByGroupResponse
                {
                    Groups = new Dictionary<string, List<OrderDetailsResponse>>()
                };
            }

            // Parse years to integers and validate
            var yearList = new List<int>();
            foreach (var yearStr in request.Years)
            {
                if (int.TryParse(yearStr, out var year) && year > 0)
                {
                    yearList.Add(year);
                }
            }

            if (!yearList.Any())
            {
                return new RevenuesByGroupResponse
                {
                    Groups = new Dictionary<string, List<OrderDetailsResponse>>()
                };
            }

            // Build filter expression for the years
            var filterExpression = BuildExpression(yearList);

            var includeExpressions = new Expression<Func<Order, object>>[]
            {
                x => x.OrderItems
            };

            // Get all orders matching the years
            var orders = await _repository.GetAllAsync(
                filterExpression: filterExpression,
                includeExpressions: includeExpressions,
                cancellationToken: cancellationToken);

            // Group orders by year
            var groupedOrders = orders
                .GroupBy(order => order.CreatedAt.Year)
                .Where(g => yearList.Contains(g.Key))
                .ToDictionary(
                    g => g.Key.ToString(),
                    g => g.Select(order => order.ToResponse()).ToList()
                );

            // Ensure all requested years are in the dictionary (even if empty)
            var responseData = new Dictionary<string, List<OrderDetailsResponse>>();
            foreach (var year in yearList)
            {
                var yearKey = year.ToString();
                responseData[yearKey] = groupedOrders.ContainsKey(yearKey)
                    ? groupedOrders[yearKey]
                    : new List<OrderDetailsResponse>();
            }

            return new RevenuesByGroupResponse
            {
                Groups = responseData
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving revenues by years: {ErrorMessage}", ex.Message);
            throw;
        }
    }

    private static Expression<Func<Order, bool>> BuildExpression(List<int> years)
    {
        var filterExpression = ExpressionBuilder.New<Order>();

        // Filter out deleted orders
        filterExpression = filterExpression.And(order => !order.IsDeleted);

        // Build date range filter for all years
        // Get the earliest start date and latest end date
        var minYear = years.Min();
        var maxYear = years.Max();

        // Start date: January 1st of the earliest year at 00:00:00 UTC
        var startDate = DateTime.SpecifyKind(new DateTime(minYear, 1, 1), DateTimeKind.Utc);

        // End date: January 1st of the year after the latest year (exclusive) at 00:00:00 UTC
        var endDate = DateTime.SpecifyKind(new DateTime(maxYear + 1, 1, 1), DateTimeKind.Utc);

        // Filter orders within the date range
        filterExpression = filterExpression.And(order => order.CreatedAt >= startDate && order.CreatedAt < endDate);

        return filterExpression;
    }
}
