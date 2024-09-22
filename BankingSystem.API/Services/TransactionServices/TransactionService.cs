using BankingSystem.API.Models;

namespace BankingSystem.API.Services.TransactionServices;

public class TransactionService : ITransactionService
{
    private readonly ApplicationDbContext _context;

    public TransactionService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> DepositAccount(int accountId, decimal amount)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var account = await _context.Accounts.FindAsync(accountId);
            if (account == null) { return false; }

            await AddTransaction(accountId, amount, account, AccountTransactionType.Overførsel);

            account.Balance += amount;

            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
            return true;

        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            return false;
        }

    }

    public async Task<bool> WithDrawFromAccount(int accountId, decimal amount)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var account = await _context.Accounts.FindAsync(accountId);

            if (account == null || account.Balance < amount) { return false; };

            await AddTransaction(accountId, amount, account, AccountTransactionType.Hævning);

            account.Balance -= amount;

            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
            return true;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            return false;
        }

    }


    private async Task AddTransaction(int accountId, decimal amount, Account? account, AccountTransactionType accountTransactionType)
    {
        decimal balancebeforetransaction = account.Balance;

        AccountTransaction accountTransaction = new()
        {
            TransferedAmmount = amount,
            AccountId = accountId,
            Account = account,
            CreatedDate = DateTime.UtcNow,
            BalanceBeforeTransaction = balancebeforetransaction,
            AccountTransactionType = accountTransactionType,
        };
        await _context.Transactions.AddAsync(accountTransaction);
    }
}
