using BankingSystem.API.Models;
using BankingSystem.API.Services.AccountServices;
using BankingSystem.API;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BankingSystem.API.Services.CrudTransactions;
using System.Transactions;
using BankingSystem.API.Services.TransactionServices;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BankingSystem.Testing.TestTransactions;

public class TransactionsTests
{
    private readonly ApplicationDbContext _context;
    private readonly TransactionCRUD _service;
    private readonly PasswordHasher<User> _passwordHasher;
    private readonly TransactionService _transactionService;

    public TransactionsTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "TestDb").ConfigureWarnings(warnings => warnings.Ignore(InMemoryEventId.TransactionIgnoredWarning)).Options;

        _context = new ApplicationDbContext(options);
        _service = new TransactionCRUD(_context);
        _passwordHasher = new PasswordHasher<User>();
        _transactionService = new TransactionService(_context);
    }

    private async Task<User> CreateAndSaveUserAsync(string email, string password)
    {
        var user = new User { Email = email };
        user.PasswordHash = _passwordHasher.HashPassword(user, password);
        await _context.AddAsync(user);
        await _context.SaveChangesAsync();
        return user;
    }

    private async Task<Account> CreateAccountWithUser()
    {
        var user = await CreateAndSaveUserAsync("Hkjasjkfajksfkj@mail.com", "HelloABc123£#");

        var account = new Account
        {
            AccountName = "Test",
            AccountType = AccountType.BudgetKonto,
            Balance = 1,
            User = user,
            UserId = user.Id,
        };

        await _context.AddAsync(account);
        await _context.SaveChangesAsync();
        return account;
    }

    [Fact]
    public async Task DepositAccount_ValidAccount_UpdatesBalance()
    {
        // Arrange
        Account account = await CreateAccountWithUser();

        decimal depositAmount = 150.00m;
        
        // Act
        var result = await _transactionService.DepositAccount(account.AccountId, depositAmount);

        // Assert
        //Transaction completed
        Assert.True(result); 

        //Arrange
        //Get accountbalance after deposit
        decimal accountBalance = account.Balance;
        
        //Act
        // Get the updated account
        var updatedAccount = await _context.Accounts.FindAsync(account.AccountId);

        //assert
        Assert.NotNull(updatedAccount);
        Assert.Equal(accountBalance, updatedAccount.Balance); 
    }

    [Fact]
    public async Task WithDrawAccount_ValidAccount_UpdatesBalance()
    {
        // Arrange
        Account account = await CreateAccountWithUser();
        account.Balance = 500m;
        decimal WithdrawedAmount = 150.00m;

        // Act
        var result = await _transactionService.WithDrawFromAccount(account.AccountId, WithdrawedAmount);

        // Assert
        //Transaction completed
        Assert.True(result);


        //Arrange
        //Get accountbalance after deposit
        decimal accountBalance = account.Balance;

        //act
        // Get the updated account
        var updatedAccount = await _context.Accounts.FindAsync(account.AccountId);

        //assert
        Assert.NotNull(updatedAccount);
        Assert.Equal(accountBalance, updatedAccount.Balance);
    }

    [Fact]
    public async Task Test_GetAllTransactions()
    {
        //Arrange
        var account = await CreateAccountWithUser();
        
        await CreateTransaction(account);

        //act

        var result = await _service.GetAllTransaction(account.AccountId);

        //assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.All(result, t => Assert.Equal(account.AccountId, t.AccountId));
    }

    private async Task CreateTransaction(Account account)
    {
        var transaction = new AccountTransaction
        {
            Account = account,
            AccountId = account.AccountId,
            BalanceBeforeTransaction = account.Balance,
            TransferedAmmount = 200,
            CreatedDate = DateTime.UtcNow,
            AccountTransactionType = AccountTransactionType.Overførsel,

        };

        await _context.Transactions.AddAsync(transaction);
        await _context.SaveChangesAsync();
    }
}
