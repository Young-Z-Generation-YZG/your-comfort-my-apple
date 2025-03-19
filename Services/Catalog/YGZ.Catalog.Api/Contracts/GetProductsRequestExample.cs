using NSwag.Generation.Processors.Contexts;
using NSwag.Generation.Processors;
using YGZ.Catalog.Api.Controllers;
using NJsonSchema;

namespace YGZ.Catalog.Api.Contracts;

public class GetProductsRequestExample : IOperationProcessor
{
    public bool Process(OperationProcessorContext context)
    {
        // Check if this is the GetOrders method in OrderingController
        if (context.ControllerType == typeof(ProductController))
        {
            var operation = context.OperationDescription.Operation;

            // Set example for _page parameter
            var pageParam = operation.Parameters.FirstOrDefault(p => p.Name == "_page");
            if (pageParam != null)
            {
                pageParam.Example = 1; // Example value
                pageParam.Description = "Valid value from 1 - 100";
            }

            // Set example for _limit parameter
            var limitParam = operation.Parameters.FirstOrDefault(p => p.Name == "_limit");
            if (limitParam != null)
            {
                limitParam.Example = 10;
                limitParam.Description = "Valid value from 1 - 100";
            }

            // Set example for _code parameter
            var codeParam = operation.Parameters.FirstOrDefault(p => p.Name == "_productColor");
            if (codeParam != null)
            {
                codeParam.Example = "ultramarine";
            }

            // Set example for _status parameter
            var statusParam = operation.Parameters.FirstOrDefault(p => p.Name == "_productStorage");
            if (statusParam != null)
            {
                statusParam.Example = "256";
            }

            var priceFromParam = operation.Parameters.FirstOrDefault(p => p.Name == "_priceFrom");
            if (priceFromParam != null)
            {
                priceFromParam.Example = "100";
            }

            var priceToParam = operation.Parameters.FirstOrDefault(p => p.Name == "_priceTo");
            if (priceToParam != null)
            {
                priceToParam.Example = "1000";
            }

            var sortParam = operation.Parameters.FirstOrDefault(p => p.Name == "_priceSort");
            if (sortParam != null)
            {
                sortParam.Example = "ASC";
            }
        }
        return true; // Continue processing other processors
    }
}
