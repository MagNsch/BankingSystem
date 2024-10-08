﻿using BankingSystem.API.Models;
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

    public async Task<bool> DeleteAccount(int id)
    {
        var account = await GetAccount(id);

        if (account == null)
        {
            return false;
        }

        _context.Accounts.Remove(account);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<Account> GetAccount(int id)
    {
        //var account = await _context.Accounts
        //                            .AsNoTracking()
        //                            .FirstOrDefaultAsync(a => a.AccountId == id && a.UserId == userId);

        if (id == 0)
        {
            throw new Exception("Id is 0");
        }
        var account = await _context.Accounts.AsNoTracking().FirstAsync(a => a.AccountId == id);
        return account;
    }

    public async Task<IEnumerable<Account>> GetAllAccounts(string userId)
    {
        //if(string.IsNullOrEmpty(userId))
        //{
        //    throw new Exception("userId not found");
        //}

        var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId);

        //if(user == null)
        //{
        //    throw new Exception("User not found");
        //}

        var accounts = await _context.Accounts.AsNoTracking().Where(a => a.UserId == user.Id).AsNoTracking().ToListAsync();

        return accounts;
    }
}
