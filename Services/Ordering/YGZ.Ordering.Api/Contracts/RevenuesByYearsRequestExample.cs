using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;
using YGZ.Ordering.Api.Controllers;

namespace YGZ.Ordering.Api.Contracts;

public class RevenuesByYearsRequestExample : IOperationProcessor
{
    public bool Process(OperationProcessorContext context)
    {
        // Check if this is the GetRevenuesByYears method in DashboardController
        if (context.ControllerType == typeof(DashboardController) &&
            context.MethodInfo.Name == "GetRevenuesByYears")
        {
            var operation = context.OperationDescription.Operation;

            // Set example for _years parameter
            var yearsParam = operation.Parameters.FirstOrDefault(p => p.Name == "_years");
            if (yearsParam != null)
            {
                yearsParam.Example = new List<string> { "2024", "2025" };
            }
        }

        return true; // Continue processing other processors
    }
}
