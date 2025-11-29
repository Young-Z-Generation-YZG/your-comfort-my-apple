using NJsonSchema.Generation;

namespace YGZ.Identity.Api.Contracts.Auth;


public class RegisterRequestExample : ISchemaProcessor
{
    public void Process(SchemaProcessorContext context)
    {
        if (context.ContextualType.Type == typeof(RegisterRequest))
        {
            context.Schema.Example = new
            {
                first_name = "Bach",
                last_name = "Le",
                email = "lov3rinve146@gmail.com",
                password = "password",
                confirm_password = "password",
                phone_number = "0123456789",
                birth_date = new DateTime(2003, 8, 16, 0, 0, 0, DateTimeKind.Utc).AddHours(7), // UTC +7
                country = "Việt Nam"
            };
        }
    }
}