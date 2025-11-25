using System.Text.Json.Serialization;
using NJsonSchema.Generation;

namespace YGZ.Identity.Api.Contracts.Addresses;

public class AddAddressRequestExample : ISchemaProcessor
{
    public void Process(SchemaProcessorContext context)
    {
        if (context.ContextualType.Type == typeof(AddAddressRequest))
        {
            context.Schema.Example = new
            {
                label = "Home",
                contact_name = "Bach Le",
                contact_phone_number = "0123456789",
                address_line = "123 Nguyen Van Cu",
                district = "District 1",
                province = "Ho Chi Minh City",
                country = "Việt Nam"
            };
        }
    }
}