using MongoDB.Bson.Serialization.Attributes;
using YGZ.BuildingBlocks.Shared.Contracts.ValueObjects;
using YGZ.Catalog.Domain.Core.Primitives;

namespace YGZ.Catalog.Domain.Tenants.ValueObjects;

public class BranchManager : ValueObject
{
    [BsonElement("user_id")]
    public string UserId { get; init; }

    [BsonElement("name")]
    public string Name { get; init; }

    [BsonElement("email")]
    public string Email { get; init; }

    [BsonElement("phone_number")]
    public string PhoneNumber { get; init; }

    private BranchManager(string userId, string name, string email, string phoneNumber)
    {
        UserId = userId;
        Name = name;
        Email = email;
        PhoneNumber = phoneNumber;
    }

    public static BranchManager Create(string userId, string name, string email, string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            throw new ArgumentException("User ID cannot be null or empty", nameof(userId));
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name cannot be null or empty", nameof(name));
        }

        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("Email cannot be null or empty", nameof(email));
        }

        if (string.IsNullOrWhiteSpace(phoneNumber))
        {
            throw new ArgumentException("Phone number cannot be null or empty", nameof(phoneNumber));
        }

        return new BranchManager(userId, name, email, phoneNumber);
    }

    public BranchManagerResponse ToResponse()

    {
        return new BranchManagerResponse
        {
            Id = UserId,
            Name = Name,
            Email = Email,
            PhoneNumber = PhoneNumber
        };
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return UserId;
        yield return Name;
        yield return Email;
        yield return PhoneNumber;
    }
}
