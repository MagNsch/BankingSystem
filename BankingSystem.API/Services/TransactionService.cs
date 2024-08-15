using BankingSystem.API.Models;

namespace BankingSystem.API.Services;

public class TransactionService : ITransactionService
{
    private readonly ApplicationDbContext _context;

    public TransactionService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> DepositAccount(int accountId, decimal amount)
    {
        var account = await _context.Accounts.FindAsync(accountId);

        if (account == null) { return false; }

        account.Balance += amount;

        await _context.SaveChangesAsync();
        return true;
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
