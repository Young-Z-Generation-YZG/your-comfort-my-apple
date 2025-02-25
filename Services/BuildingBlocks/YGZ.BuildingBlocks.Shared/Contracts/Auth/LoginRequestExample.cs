
using Swashbuckle.AspNetCore.Filters;

namespace YGZ.BuildingBlocks.Shared.Contracts.Auth;

public class LoginRequestExample : IExamplesProvider<LoginRequest>
{
    public LoginRequest GetExamples()
    {
        return new("lov3rinve146@gmail.com", "password");
    }
}
