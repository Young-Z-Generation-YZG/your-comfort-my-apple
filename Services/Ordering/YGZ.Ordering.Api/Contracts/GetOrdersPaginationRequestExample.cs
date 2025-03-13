using NJsonSchema.Generation;
using NSwag.Generation.Processors.Contexts;
using NSwag.Generation.Processors;
using YGZ.Ordering.Api.Controllers;

namespace YGZ.Ordering.Api.Contracts;

public class GetOrdersPaginationRequestExample : IOperationProcessor
{
    public bool Process(OperationProcessorContext context)
    {
        // Check if this is the GetOrders method in OrderingController
        if (context.ControllerType == typeof(OrderingController) &&
            context.MethodInfo.Name == "GetOrders")
        {
            var operation = context.OperationDescription.Operation;

            // Set example for _page parameter
            var pageParam = operation.Parameters.FirstOrDefault(p => p.Name == "_page");
            if (pageParam != null)
            {
                pageParam.Example = 1;
            }

            // Set example for _limit parameter
            var limitParam = operation.Parameters.FirstOrDefault(p => p.Name == "_limit");
            if (limitParam != null)
            {
                limitParam.Example = 10;
            }

            // Set example for _order_name parameter
            var orderNameParam = operation.Parameters.FirstOrDefault(p => p.Name == "_orderName");
            if (orderNameParam != null)
            {
                orderNameParam.Example = "Bach Le";
            }

            // Set example for _code parameter
            var codeParam = operation.Parameters.FirstOrDefault(p => p.Name == "_orderCode");
            if (codeParam != null)
            {
                codeParam.Example = "#645006";
            }

            // Set example for _status parameter
            var statusParam = operation.Parameters.FirstOrDefault(p => p.Name == "_orderStatus");
            if (statusParam != null)
            {
                statusParam.Example = "PAID";
            }
        }
        return true; // Continue processing other processors
    }
}