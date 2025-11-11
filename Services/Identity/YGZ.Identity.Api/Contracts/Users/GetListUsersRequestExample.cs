using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;
using YGZ.Identity.Api.Controllers;

namespace YGZ.Identity.Api.Contracts.Users;

public class GetListUsersRequestExample : IOperationProcessor
{
    public bool Process(OperationProcessorContext context)
    {
        // Check if this is the GetListUsers method in UserController
        if (context.ControllerType == typeof(UserController) &&
            context.MethodInfo.Name == "GetListUsers")
        {
            var operation = context.OperationDescription.Operation;

            // Set example for _roles parameter
            var rolesParam = operation.Parameters.FirstOrDefault(p => p.Name == "_roles");
            if (rolesParam != null)
            {
                rolesParam.Example = new List<string> { "ADMIN", "STAFF" };
            }
        }

        return true; // Continue processing other processors
    }
}
