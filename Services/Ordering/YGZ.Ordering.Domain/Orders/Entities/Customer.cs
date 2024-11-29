
using YGZ.Ordering.Domain.Core.Primitives;
using YGZ.Ordering.Domain.Orders.ValueObjects;

namespace YGZ.Ordering.Domain.Orders.Entities;

public class Customer : Entity<CustomerId>
{
    public string Name { get; private set; } = default!;
    public string Email { get; private set; } = default!;

    public Customer(CustomerId id, string name, string email) : base(id)
    {
        Name = name;
        Email = email;
    }

    public static Customer CreateNew(string name, string email)
    {
        return new Customer(CustomerId.CreateNew(), name, email);
    }
}
