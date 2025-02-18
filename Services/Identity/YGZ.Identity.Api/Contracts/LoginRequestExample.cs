
using Swashbuckle.AspNetCore.Filters;

namespace YGZ.Identity.Api.Contracts;

public class LoginRequestExample : IExamplesProvider<LoginRequest>
{
    public LoginRequest GetExamples()
    {
        return new("lov3rinve146@gmail.com", "password");
    }
}
