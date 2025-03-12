
namespace YGZ.Ordering.Domain.Core.Abstractions;

public interface IAuditable<T>
{
    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
    public T? LastModifiedBy { get; set; }
}
