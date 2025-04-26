using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;
using YGZ.Ordering.Api.Controllers;

namespace YGZ.Ordering.Api.Contracts;

public class UpdateOrderStatusRequestExample : IOperationProcessor
{
    public bool Process(OperationProcessorContext context)
    {
        if (context.ControllerType == typeof(OrderingController))
        {
            var operation = context.OperationDescription.Operation;

            // Set example for _page parameter
            var updatedStautsParam = operation.Parameters.FirstOrDefault(p => p.Name == "_updatedStatus");
            if (updatedStautsParam != null)
            {
                List<string> statusList = ["PREPARING", "DELIVERING", "DELIVERED"];

                updatedStautsParam.Schema = new NJsonSchema.JsonSchema
                {
                    Type = NJsonSchema.JsonObjectType.String
                };
                foreach (var name in statusList)
                {
                    updatedStautsParam.Schema.Enumeration.Add(name);
                }
            }
        }

        return true;
    }
}
