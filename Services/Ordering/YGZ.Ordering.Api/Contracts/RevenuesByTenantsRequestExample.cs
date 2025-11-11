using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;
using YGZ.Ordering.Api.Controllers;

namespace YGZ.Ordering.Api.Contracts;

public class RevenuesByTenantsRequestExample : IOperationProcessor
{
    public bool Process(OperationProcessorContext context)
    {
        // Check if this is the GetRevenuesByTenants method in DashboardController
        if (context.ControllerType == typeof(DashboardController) &&
            context.MethodInfo.Name == "GetRevenuesByTenants")
        {
            var operation = context.OperationDescription.Operation;

            // Set example for _tenants parameter
            var tenantsParam = operation.Parameters.FirstOrDefault(p => p.Name == "_tenants");
            if (tenantsParam != null)
            {
                tenantsParam.Example = new List<string> { "tenant-id-1", "tenant-id-2" };
            }

            // Set example for _startDate parameter
            var startDateParam = operation.Parameters.FirstOrDefault(p => p.Name == "_startDate");
            if (startDateParam != null)
            {
                startDateParam.Example = new DateTime(2025, 1, 1);
            }

            // Set example for _endDate parameter
            var endDateParam = operation.Parameters.FirstOrDefault(p => p.Name == "_endDate");
            if (endDateParam != null)
            {
                endDateParam.Example = new DateTime(2025, 12, 31);
            }
        }

        return true; // Continue processing other processors
    }
}
