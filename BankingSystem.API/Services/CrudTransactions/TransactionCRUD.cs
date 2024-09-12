using BankingSystem.API.Models;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem.API.Services.CrudTransactions;

public class TransactionCRUD : ITransactionCRUD
{
    private readonly ApplicationDbContext _context;

    public TransactionCRUD(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<AccountTransaction?>> GetAllTransaction(int accountId)
    {
        if (accountId == 0)
        {
            return new List<AccountTransaction?>();
        }

        var transactions = await _context.Transactions
                           .Include(t => t.Account)
                           .Where(t => t.AccountId == accountId).AsNoTracking()
                           .ToListAsync();
        return transactions;
    }
}
