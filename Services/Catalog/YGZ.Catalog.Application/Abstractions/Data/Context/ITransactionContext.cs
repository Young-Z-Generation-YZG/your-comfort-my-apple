using MongoDB.Driver;

namespace YGZ.Catalog.Application.Abstractions.Data.Context;

public interface ITransactionContext
{
    IClientSessionHandle? CurrentSession { get; }
    void SetSession(IClientSessionHandle session);
    void ClearSession();
}
