
using System.Text.Json.Serialization;
using YGZ.BuildingBlocks.Shared.Utils;

namespace YGZ.BuildingBlocks.Shared.Contracts.Common;

[JsonConverter(typeof(SnakeCaseJsonSerializer))]
public sealed record StorageResponse
{
    required public string Name { get; init; }
    required public int Order { get; init; }
}
