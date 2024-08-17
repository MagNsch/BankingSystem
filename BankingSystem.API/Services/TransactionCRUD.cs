using BankingSystem.API.Models;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem.API.Services;

public class TransactionCRUD : ITransactionCRUD
{
    private readonly ApplicationDbContext _context;

    public TransactionCRUD(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<AccountTransaction?>> GetAllTransaction(int accountId)
    {
        //return await _context.Transactions.Where(a => a.AccountId == accountId).ToListAsync();

        var transactions = await _context.Transactions
                           .Include(t => t.Account)
                           .Where(t => t.AccountId == accountId)
                           .ToListAsync();

        return transactions;
    }
}
