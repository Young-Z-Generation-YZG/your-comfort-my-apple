
namespace YGZ.Catalog.Domain.Core.Abstractions.Data;

public interface IUnitOfWork : IDisposable
{
    Task<bool> Commit();
}