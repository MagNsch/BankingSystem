using BankingSystem.API.Models;

namespace BankingSystem.UI.RestService.Accounts;

public interface IRestServiceClient
{
    Task<IEnumerable<Account>> GetAllAccounts();
    Task<Account> GetAccountById(int id);
    Task<Account> CreateAccount(Account account);
    Task<bool> DeleteAccount(int id);
}
