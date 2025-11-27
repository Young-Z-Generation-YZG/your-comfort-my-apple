using NJsonSchema.Generation;
using NSwag.Generation.Processors.Contexts;
using NSwag.Generation.Processors;
using YGZ.Ordering.Api.Controllers;

namespace YGZ.Ordering.Api.Contracts;

public class GetNotificationsRequestExample : IOperationProcessor
{
    public bool Process(OperationProcessorContext context)
    {
        // Check if this is the GetNotifications method in NotificationController
        if (context.ControllerType == typeof(NotificationController) &&
            context.MethodInfo.Name == "GetNotifications")
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

            // Set example for _types parameter
            var typesParam = operation.Parameters.FirstOrDefault(p => p.Name == "_types");
            if (typesParam != null)
            {
                typesParam.Example = new[] { "ORDER_STATUS_UPDATE", "ORDER_CREATED" };
            }

            // Set example for _statuses parameter
            var statusesParam = operation.Parameters.FirstOrDefault(p => p.Name == "_statuses");
            if (statusesParam != null)
            {
                statusesParam.Example = new[] { "PENDING", "SENT" };
            }

            // Set example for _isRead parameter
            var isReadParam = operation.Parameters.FirstOrDefault(p => p.Name == "_isRead");
            if (isReadParam != null)
            {
                isReadParam.Example = false;
            }
        }
        return true; // Continue processing other processors
    }
}
