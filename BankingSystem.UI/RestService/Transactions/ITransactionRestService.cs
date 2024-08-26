using BankingSystem.API.Models;

namespace BankingSystem.UI.RestService.Transactions;

public interface ITransactionRestService
{
    Task<bool> DepositToAccount(int accountId, decimal amount);
    Task<bool> WithDrawFromAccount(int accountId, decimal amount);
}
