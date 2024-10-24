using Swashbuckle.AspNetCore.Filters;

namespace YGZ.Catalog.Api.Test;

public class PersonExample : IExamplesProvider<Person>
{
    public Person GetExamples()
    {
        return new Person()
        {
            Id = 1,
            Name = "test 123",
            Contact = "test"
        };
    }
}
