namespace YGZ.BuildingBlocks.Shared.Abstractions.Data;

public interface IAuditable
{
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }
}
