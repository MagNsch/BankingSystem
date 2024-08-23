using BankingSystem.API.Models;

namespace BankingSystem.UI.RestService;

public interface IRestServiceClient
{
    Task<IEnumerable<Account>> GetAllAccounts(string userId);
    Task<Account> GetAccountById(int id);
    Task<Account> CreateAccount(Account account);
}
