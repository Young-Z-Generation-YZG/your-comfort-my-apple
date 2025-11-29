using System.Text.Json.Serialization;
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
                label = "Work",
                contact_name = "Le Xuan Bach",
                contact_phone_number = "=0333284890",
                address_line = "Hoang Dieu 2",
                district = "Thu Duc",
                province = "Ho Chi Minh city",
                country = "Việt Nam"
            };
        }
    }
}
