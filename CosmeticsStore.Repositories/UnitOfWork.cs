using CosmeticsStore.Repositories.Interfaces;
using CosmeticsStore.Repositories.Models;
using Microsoft.EntityFrameworkCore.Storage;

namespace CosmeticsStore.Repositories;

/// <summary>
/// Unit of Work implementation
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly CosmeticsDbContext _context;
    private IDbContextTransaction? _transaction;
    
    private ICosmeticInformationRepository? _cosmeticInformations;
    private ISystemAccountRepository? _systemAccounts;

    public UnitOfWork(CosmeticsDbContext context)
    {
        _context = context;
    }

    public ICosmeticInformationRepository CosmeticInformations
    {
        get
        {
            _cosmeticInformations ??= new CosmeticInformationRepository(_context);
            return _cosmeticInformations;
        }
    }

    public ISystemAccountRepository SystemAccounts
    {
        get
        {
            _systemAccounts ??= new SystemAccountRepository(_context);
            return _systemAccounts;
        }
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}
