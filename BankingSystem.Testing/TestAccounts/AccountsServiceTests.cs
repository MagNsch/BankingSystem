using BankingSystem.API;
using BankingSystem.API.Models;
using BankingSystem.API.Services.AccountServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem.Testing.TestAccounts;

public class AccountsServiceTests
{
    private readonly ApplicationDbContext _context;
    private readonly AccountService _service;
    private readonly PasswordHasher<User> _passwordHasher;

    public AccountsServiceTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "TestDb").Options;

        _context = new ApplicationDbContext(options);
        _service = new AccountService(_context);
        _passwordHasher = new PasswordHasher<User>();
    }




    [Fact]
    public async Task CreateUser_FindInMemoryDatabase()
    {
        //arrange
        var user = await CreateAndSaveAsync("magnu@mail.com", "Hansen!2");

        var account = new Account
        {
            AccountName = "Test Account",
            AccountType = AccountType.PrimærKonto,
            Balance = 0,
            User = user,
            UserId = user.Id,
        };

        //act

        await _service.CreateAccount(account);

        var savedAccount = await _context.Accounts.FindAsync(account.AccountId);

        //assert

        Assert.NotNull(savedAccount);
        Assert.Equal("Test Account", savedAccount.AccountName);
        Assert.Equal(user.Id, savedAccount.UserId);
    }

    [Fact]
    public async Task GetUserById_InDb()
    {
        var newAccount = await Create_Account_Async();
        //arrange
        int accountId = 1;

        //act
        var account = await _service.GetAccount(newAccount.AccountId);

        //assert
        Assert.NotNull(account);
        Assert.Equal(accountId, account.AccountId);
    }

    [Fact]
    public async Task GetAllAccounts_InDb()
    {
        var user = await CreateAndSaveAsync("Hello@gmail.com", "123!Abcdef");

        await AddAccounts(user);

        var accounts = await _service.GetAllAccounts(user.Id);

        Assert.Equal(3, accounts.Count());
        Assert.True(accounts.Any());
        Assert.NotNull(accounts);
    }

    [Fact]
    public async Task Delete_Account_inDB()
    {
        var account_created_in_db_has_id_1 = 1;
        var isDeleted = await _service.DeleteAccount(account_created_in_db_has_id_1);
        _context.SaveChanges();

        Assert.True(isDeleted);

    }
    private async Task<Account> Create_Account_Async()
    {
        var user = await CreateAndSaveAsync("Hello@gmail.com", "123!Abcdef");

        var account = new Account
        {
            AccountName = "Test Account",
            AccountType = AccountType.PrimærKonto,
            Balance = 0,
            User = user,
            UserId = user.Id,
        };

        await _context.AddAsync(account);
        await _context.SaveChangesAsync();

        return account;
    }


    private async Task AddAccounts(User user)
    {
        var account1 = new Account
        {
            AccountName = "Test User",
            AccountType = AccountType.PrimærKonto,
            Balance = 0,
            User = user,
            UserId = user.Id,
        };

        var account2 = new Account
        {
            AccountName = "Test User",
            AccountType = AccountType.PrimærKonto,
            Balance = 0,
            User = user,
            UserId = user.Id,
        };

        var account3 = new Account
        {
            AccountName = "Test User",
            AccountType = AccountType.PrimærKonto,
            Balance = 0,
            User = user,
            UserId = user.Id,
        };

        await _context.AddRangeAsync(account1, account2, account3);

        await _context.SaveChangesAsync();
    }

    private async Task<User> CreateAndSaveAsync(string email, string password)
    {
        var user = new User { Email = email };
        user.PasswordHash = _passwordHasher.HashPassword(user, password);
        await _context.AddAsync(user);
        await _context.SaveChangesAsync();
        return user;
    }
}
