using System.Text.Json.Serialization;
using YGZ.BuildingBlocks.Shared.Utils;
namespace YGZ.BuildingBlocks.Shared.Contracts.Catalogs;

[JsonConverter(typeof(SnakeCaseJsonSerializer))]
public sealed record ChatbotMessageResponse
{
    public required string Content { get; init; }
    public required string Role { get; init; }
}
