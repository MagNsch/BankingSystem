using BankingSystem.API.Models;

namespace BankingSystem.API.Services.AccountServices;

public interface IAccountService
{
    Task<Account> CreateAccount(Account account);
    Task<IEnumerable<Account>> GetAllAccounts(string userId);
    Task<Account> GetAccount(int id);

    Task<Account> DeleteAccount(int id);
}
