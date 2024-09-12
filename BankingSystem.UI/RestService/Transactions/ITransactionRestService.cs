using BankingSystem.API.Models;
using System.Collections.Generic;
using System.Transactions;

namespace BankingSystem.UI.RestService.Transactions;

public interface ITransactionRestService
{
    Task<bool> DepositToAccount(int accountId, decimal amount);
    Task<bool> WithDrawFromAccount(int accountId, decimal amount);
    Task<IEnumerable<AccountTransaction>> GetAllTransactionsById(int accountId);
    
}
