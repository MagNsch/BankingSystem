using BankingSystem.API.Models;

namespace BankingSystem.UI.RestService;

public interface IRestServiceClient
{
    Task<IEnumerable<Account>> GetAllAccounts();
}
