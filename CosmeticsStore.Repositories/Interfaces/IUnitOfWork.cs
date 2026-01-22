namespace CosmeticsStore.Repositories.Interfaces;

/// <summary>
/// Unit of Work pattern - manages transactions across multiple repositories
/// </summary>
public interface IUnitOfWork : IDisposable
{
    ICosmeticInformationRepository CosmeticInformations { get; }
    ISystemAccountRepository SystemAccounts { get; }
    
    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}
