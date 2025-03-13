
using System.Linq.Expressions;
using YGZ.Ordering.Application.Orders;
using YGZ.Ordering.Domain.Core.Enums;
using YGZ.Ordering.Domain.Orders.ValueObjects;

namespace YGZ.Ordering.Infrastructure.Persistence.Expressions;


public static class ExpressionBuilder
{
    public static Expression<Func<T, bool>> New<T>()
    {
        return x => true;
    }

    public static Expression<Func<T, bool>> New<T>(Expression<Func<T, bool>> expression)
    {
        return expression;
    }

    public static Expression<Func<T, bool>> And<T>(
        this Expression<Func<T, bool>> left,
        Expression<Func<T, bool>> right)
    {
        return Expression.Lambda<Func<T, bool>>(
            Expression.AndAlso(
                left.Body,
                Expression.Invoke(right, left.Parameters[0])), left.Parameters[0]);
    }

    public static Expression<Func<T, bool>> Or<T>(
        this Expression<Func<T, bool>> left,
        Expression<Func<T, bool>> right)
    {
        return Expression.Lambda<Func<T, bool>>(
            Expression.OrElse(
                left.Body,
                Expression.Invoke(right, left.Parameters[0])), left.Parameters[0]);
    }
}

//public static class OrderFilterExpressionBuilder
//{
//    public static Expression<Func<Order, bool>> BuildFilterExpression(string? orderName, string? code, string? status)
//    {
//        Expression<Func<Order, bool>> filterExpression = order => true;

//        if (!string.IsNullOrWhiteSpace(orderName))
//        {
//            filterExpression = Combine(filterExpression, order => order.ShippingAddress.ContactName.Contains(orderName));
//        }
//        if (!string.IsNullOrWhiteSpace(code))
//        {
//            filterExpression = Combine(filterExpression, order => order.Code.Equals(Code.Of(code)));
//        }
//        if (!string.IsNullOrWhiteSpace(status))
//        {
//            filterExpression = Combine(filterExpression, order => order.Status == OrderStatusEnum.FromName(status, false));
//        }

//        return filterExpression;
//    }

//    private static Expression<Func<Order, bool>> And(Expression<Func<Order, bool>> left, Expression<Func<Order, bool>> right)
//    {
//        // Create a parameter for the Order type
//        var parameter = Expression.Parameter(typeof(Order));

//        // Combine the existing filter and new condition with AND
//        var body = Expression.AndAlso(
//            Expression.Invoke(baseExpression, parameter),
//            Expression.Invoke(newExpression, parameter));

//        // Return the combined expression
//        return Expression.Lambda<Func<Order, bool>>(body, parameter);
//    }

//    private static Expression<Func<Order, bool>> Or(Expression<Func<Order, bool>> left, Expression<Func<Order, bool>> right)
//    {
//        // Create a parameter for the Order type
//        var parameter = Expression.Parameter(typeof(Order));

//        // Combine the existing filter and new condition with AND
//        var body = Expression.AndAlso(
//            Expression.Invoke(baseExpression, parameter),
//            Expression.Invoke(newExpression, parameter));

//        // Return the combined expression
//        return Expression.Lambda<Func<Order, bool>>(body, parameter);
//    }
//}
