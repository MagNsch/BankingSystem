namespace BankingSystem.API.Services.TransactionServices;

public interface ITransactionService
{
    Task<bool> DepositAccount(int accountId, decimal amount);

    Task<bool> WithDrawFromAccount(int accountId, decimal amount);
}
