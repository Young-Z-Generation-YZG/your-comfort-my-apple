namespace YGZ.Catalog.Domain.Core.Abstractions;

public interface ISoftDelete
{
    public bool Is_deleted { get; set; }

    public DateTime Deleted_at { get; set; }
}
