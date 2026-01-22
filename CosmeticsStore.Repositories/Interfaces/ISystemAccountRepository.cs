using CosmeticsStore.Repositories.Models;

namespace CosmeticsStore.Repositories.Interfaces;

public interface ISystemAccountRepository
{
    Task<SystemAccount> Login(string email, string password);
}
