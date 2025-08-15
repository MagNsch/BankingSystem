using BankingSystem.API.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem.API.Services.AccountServices;

public class AccountService : IAccountService
{
    private readonly ApplicationDbContext _context;

    public AccountService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task TransferFunds(int fromAccountId, int toAccountId, decimal amount)
    {
        var fromAccount = await _context.Accounts.FindAsync(fromAccountId);

        var toAccount = await _context.Accounts.FindAsync(toAccountId);

        if (fromAccount == null || toAccount == null)
        {
            throw new Exception("An account was not found");
        }

        if (fromAccount.Balance < amount)
        {
            throw new Exception("Balance is too low");
        }

        fromAccount.Balance -= amount;
        toAccount.Balance += amount;

        await _context.SaveChangesAsync();
    }



    public async Task<Account> CreateAccount(Account account)
    {
        User? user = _context.Users.AsNoTracking().FirstOrDefault(u => u.Id == account.UserId);

        if (string.IsNullOrEmpty(user.Id)) { throw new Exception("User was not found"); }

        _context.Accounts.Add(account);

        await _context.SaveChangesAsync();

        return account;
    }

    public async Task<bool> DeleteAccount(int id, string userId)
    {
        var account = await GetAccount(id, userId);

        if (account == null)
        {
            return false;
        }

        _context.Accounts.Remove(account);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<Account> GetAccount(int id, string userId)
    {
        var account = await _context.Accounts.AsNoTracking().FirstOrDefaultAsync(a => a.UserId == userId && a.AccountId == id);
        return account;
    }

    public async Task<IEnumerable<Account>> GetAllAccounts(string userId)
    {
        if (string.IsNullOrEmpty(userId))
        {
            throw new Exception("User cannot be logged in");
        }
        var user = await _context.Users.AsNoTracking().Include(u => u.Accounts.OrderBy(a => a.AccountId)).FirstOrDefaultAsync(u => u.Id == userId);
        return user.Accounts;
    }
}
