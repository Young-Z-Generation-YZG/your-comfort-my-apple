

using System.Text.Json.Serialization;
using YGZ.BuildingBlocks.Shared.Utils;

namespace YGZ.BuildingBlocks.Shared.Contracts.Common;

[JsonConverter(typeof(SnakeCaseJsonSerializer))]
public sealed record CloudinaryImageResponse
{
    public required string OriginalFilename { get; init; }
    public required string Format { get; init; }
    public required string PublicId { get; init; }
    public required string DisplayName { get; init; }
    public required string SecureUrl { get; init; }
    public required decimal Length { get; init; }
    public required decimal Bytes { get; init; }
    public required decimal Width { get; init; }
    public required decimal Height { get; init; }
}