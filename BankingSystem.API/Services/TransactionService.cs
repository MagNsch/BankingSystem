using BankingSystem.API.Models;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;

namespace BankingSystem.API.Services;

public class TransactionService : ITransactionService
{
    private readonly ApplicationDbContext _context;

    public TransactionService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<AccountTransaction?>> GetAllTransaction(int accountId)
    {
        return await _context.Transactions.Where(a => a.AccountId == accountId).ToListAsync();
    }


    public async Task<bool> DepositAccount(int accountId, decimal amount)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var account = await _context.Accounts.FindAsync(accountId);
            if (account == null) { return false; }
            
            await AddTransaction(accountId, amount, account);

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

    private async Task AddTransaction(int accountId, decimal amount, Account? account)
    {
        decimal balancebeforetransaction = account.Balance;

        AccountTransaction accountTransaction = new()
        {
            TransferedAmmount = amount,
            AccountId = accountId,
            Account = account,
            CreatedDate = DateTime.UtcNow,
            BalanceBeforeTransaction = balancebeforetransaction,
            AccountTransactionType = AccountTransactionType.Overførsel,
        };
        await _context.Transactions.AddAsync(accountTransaction);
    }

    public async Task<bool> WithDrawFromAccount(int accountId, decimal amount)
    {
        var account = await _context.Accounts.FindAsync(accountId);

        if (account == null || account.Balance < amount) { return false; };


        account.Balance -= amount;

        await _context.SaveChangesAsync();
        return true;
    }
}
