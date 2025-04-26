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

            var updateStatusParam = operation.Parameters.FirstOrDefault(p => p.Name == "_updateStatus");
            if (updateStatusParam != null)
            {
                List<string> statusList = ["PREPARING", "DELIVERING", "DELIVERED"];

                updateStatusParam.Schema = new NJsonSchema.JsonSchema
                {
                    Type = NJsonSchema.JsonObjectType.String
                };
                foreach (var name in statusList)
                {
                    updateStatusParam.Schema.Enumeration.Add(name);
                }
            }
        }

        return true;
    }
}
