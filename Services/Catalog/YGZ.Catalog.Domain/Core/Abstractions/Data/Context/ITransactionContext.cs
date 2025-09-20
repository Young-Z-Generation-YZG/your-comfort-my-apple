using MongoDB.Driver;

namespace YGZ.Catalog.Domain.Core.Abstractions.Context;

public interface ITransactionContext
{
    IClientSessionHandle? CurrentSession { get; }
    void SetSession(IClientSessionHandle session);
    void ClearSession();
}
