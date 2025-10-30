
using System.Text.Json.Serialization;
using YGZ.BuildingBlocks.Shared.Utils;

namespace YGZ.BuildingBlocks.Shared.Contracts.Identity;

[JsonConverter(typeof(SnakeCaseJsonSerializer))]
public sealed record ProfileResponse
{
    public required string Id { get; init; }
    public required string UserId { get; set; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required DateTime BirthDay { get; init; }
    public required string Gender { get; init; }
    public string? ImageId { get; init; }
    public string? ImageUrl { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required DateTime UpdatedAt { get; init; }
    public string? UpdatedBy { get; init; }
    public required bool IsDeleted { get; init; }
    public DateTime? DeletedAt { get; init; }
    public string? DeletedBy { get; init; }
}
