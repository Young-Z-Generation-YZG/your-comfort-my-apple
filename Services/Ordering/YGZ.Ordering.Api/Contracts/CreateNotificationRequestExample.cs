using NJsonSchema.Generation;

namespace YGZ.Ordering.Api.Contracts;

public class CreateNotificationRequestExample : ISchemaProcessor
{
    public void Process(SchemaProcessorContext context)
    {
        if (context.ContextualType.Type == typeof(CreateNotificationRequest))
        {
            var schema = context.Schema;

            schema.Example = new
            {
                title = "New Order",
                content = "You have a new order",
                type = "ORDER_CREATED",
                status = "PENDING",
                link = "https://www.google.com"
            };
        }
    }
}
