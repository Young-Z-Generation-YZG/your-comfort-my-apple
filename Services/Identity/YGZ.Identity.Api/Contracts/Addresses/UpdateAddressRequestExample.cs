using NJsonSchema.Generation;

namespace YGZ.Identity.Api.Contracts.Addresses;

public class UpdateAddressRequestExample : ISchemaProcessor
{
    public void Process(SchemaProcessorContext context)
    {
        if (context.ContextualType.Type == typeof(UpdateAddressRequest))
        {
            context.Schema.Example = new
            {
                email = "lov3rinve146@gmail.com",
                password = "password",
            };
        }
    }
}
