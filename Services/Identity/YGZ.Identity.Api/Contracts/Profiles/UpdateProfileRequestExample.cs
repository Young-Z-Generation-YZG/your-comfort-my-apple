using System.Text.Json.Serialization;
using NJsonSchema.Generation;

namespace YGZ.Identity.Api.Contracts.Profiles;

public class UpdateProfileRequestExample : ISchemaProcessor
{
    public void Process(SchemaProcessorContext context)
    {

        if (context.ContextualType.Type == typeof(UpdateProfileRequest))
        {
            context.Schema.Example = new
            {
                first_name = "Bale",
                last_name = "Le",
                phone_number = "0333234390",
                birthday = "2003-09-16",
                gender = "OTHER",
            };
        }
    }
}