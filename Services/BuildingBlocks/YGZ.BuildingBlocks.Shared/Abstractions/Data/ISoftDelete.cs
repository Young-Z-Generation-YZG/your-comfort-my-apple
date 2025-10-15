namespace YGZ.BuildingBlocks.Shared.Abstractions.Data;

public interface ISoftDelete
{
    bool IsDeleted { get; init; }

    DateTime? DeletedAt { get; init; }

    string? DeletedBy { get; init; }
}