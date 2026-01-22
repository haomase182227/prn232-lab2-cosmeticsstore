using CosmeticsStore.Repositories.Interfaces;
using CosmeticsStore.Repositories.Models;
using Microsoft.EntityFrameworkCore;

namespace CosmeticsStore.Repositories;

public class SystemAccountRepository : ISystemAccountRepository
{
    private readonly CosmeticsDbContext _context;

    public SystemAccountRepository(CosmeticsDbContext context)
    {
        _context = context;
    }

    public async Task<SystemAccount> Login(string email, string password)
    {
        var account = await _context.SystemAccounts
            .FirstOrDefaultAsync(account => account.EmailAddress == email && account.AccountPassword == password);
        return account;
    }
}
