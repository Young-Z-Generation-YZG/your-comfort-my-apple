namespace YGZ.Discount.Domain.Abstractions.Data;

public interface ISoftDelete
{
    bool IsDeleted { get; }

    DateTime? DeletedAt { get; }

    string? DeletedBy { get; }
}