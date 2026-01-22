using CosmeticsStore.Repositories.Models;

namespace CosmeticsStore.Services.Interfaces;

public interface ISystemAccountService
{
    Task<SystemAccount> Login(string email, string password);
}
