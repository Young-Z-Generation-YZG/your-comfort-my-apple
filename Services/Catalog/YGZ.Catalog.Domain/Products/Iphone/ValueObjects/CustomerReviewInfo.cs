using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using YGZ.BuildingBlocks.Shared.Contracts.Reviews;
using YGZ.Catalog.Domain.Core.Primitives;

namespace YGZ.Catalog.Domain.Products.Iphone.ValueObjects;

public class CustomerReviewInfo : ValueObject
{
    [BsonElement("name")]
    public string Name { get; init; } = string.Empty;

    [BsonElement("avatar_image_url")]
    public string? AvatarImageUrl { get; init; }

    [BsonElement("user_id")]
    public string? UserId { get; init; }

    private CustomerReviewInfo(string name, string? avatarImageUrl, string? userId)
    {
        Name = name;
        AvatarImageUrl = avatarImageUrl;
        UserId = userId;
    }

    public static CustomerReviewInfo Create(string name, string? avatarImageUrl = null, string? userId = null)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name cannot be null or empty", nameof(name));
        }

        return new CustomerReviewInfo(name, avatarImageUrl, userId);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Name;
        if (AvatarImageUrl is not null)
        {
            yield return AvatarImageUrl;
        }
        if (UserId is not null)
        {
            yield return UserId;
        }
    }

    public CustomerReviewInfoResponse ToResponse()
    {
        return new CustomerReviewInfoResponse
        {
            Name = Name,
            AvatarImageUrl = AvatarImageUrl,
            UserId = UserId
        };
    }
}
