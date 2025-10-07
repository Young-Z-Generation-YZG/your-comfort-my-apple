﻿using System.Text.Json.Serialization;
using YGZ.BuildingBlocks.Shared.Contracts.ValueObjects;
using YGZ.BuildingBlocks.Shared.Utils;

namespace YGZ.BuildingBlocks.Shared.Contracts.Catalogs.Iphone;

[JsonConverter(typeof(SnakeCaseJsonSerializer))]
public sealed record BranchResponse
{
    required public string Id { get; init; }
    required public string TenantId { get; init; }
    required public string Name { get; init; }
    required public string Address { get; init; }
    public string? Description { get; init; }
    public BranchManagerResponse? Manager { get; init; }
}
