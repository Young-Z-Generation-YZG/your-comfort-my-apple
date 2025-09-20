namespace YGZ.Discount.Domain.Abstractions.Data;

public interface IAuditable
{
    public DateTime CreatedAt { get; }
    public DateTime UpdatedAt { get; }
    public string? UpdatedBy { get; }
}
