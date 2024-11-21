

namespace YGZ.Ordering.Domain.Core.Abstractions;

public interface ISoftDelete
{
    public bool IsDeleted { get; set; }
    public DateTime DeletedAt { get; set; }
}
