using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;
using YGZ.Identity.Api.Controllers;

namespace YGZ.Identity.Api.Contracts;

public class AccessOtpRequestExample : IOperationProcessor
{
    public bool Process(OperationProcessorContext context)
    {
        if(context.ControllerType == typeof(AuthController))
        {
            var operation = context.OperationDescription.Operation;

            // Set example for _q parameter
            var verifyParam = operation.Parameters.FirstOrDefault(p => p.Name == "_verifyType");
            if (verifyParam != null)
            {
                verifyParam.Example = "email";

                List<string> list = ["email", "resetPassword", "changeEmail", "changePhoneNumber"];

                verifyParam.Schema = new NJsonSchema.JsonSchema
                {
                    Type = NJsonSchema.JsonObjectType.String
                };
                foreach (var name in list)
                {
                    verifyParam.Schema.Enumeration.Add(name);
                }
            }
        }

        return true;
    }
}
