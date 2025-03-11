

namespace YGZ.Discount.Application.Abstractions;

public interface ISoftDelete
{
    bool IsDeleted { get; }

    DateTime? DeletedAt { get; }

    string? DeletedByUserId { get; }
}