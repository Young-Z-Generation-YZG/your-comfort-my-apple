
using System.Text.Json.Serialization;
using YGZ.BuildingBlocks.Shared.Utils;

namespace YGZ.BuildingBlocks.Shared.Contracts.Common;

[JsonConverter(typeof(SnakeCaseSerializer))]
public sealed record StorageResponse
{
    required public string StorageName { get; set; }
    required public int StorageValue { get; set; }
}
