
using System.Text.Json.Serialization;
using YGZ.BuildingBlocks.Shared.Utils;

namespace YGZ.BuildingBlocks.Shared.Contracts;

[JsonConverter(typeof(SnakeCaseSerializerConverter))]
public sealed record ProductResponse
{
    public string ProductId { get; init; }
}
