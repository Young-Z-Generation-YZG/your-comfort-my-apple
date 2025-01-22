using Swashbuckle.AspNetCore.Filters;

namespace YGZ.Identity.Api.Contracts;

public class RegisterRequestExample : IExamplesProvider<RegisterRequest>
{
    public RegisterRequest GetExamples()
    {
        return new("John", "Doe", "lov3rinve146@gmail.com", "password");
    }
}
