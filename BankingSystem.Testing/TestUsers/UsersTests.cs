using BankingSystem.API.Models;
using BankingSystem.API.Services.AccountServices;
using BankingSystem.API;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BankingSystem.API.Services.UserServices;

namespace BankingSystem.Testing.TestUsers;

public class UsersTests
{
    private readonly ApplicationDbContext _context;
    private readonly UserService _service;
    private readonly PasswordHasher<User> _passwordHasher;

    public UsersTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "TestDb").Options;

        _context = new ApplicationDbContext(options);
        _service = new UserService(_context);
        _passwordHasher = new PasswordHasher<User>();
    }

    private async Task<User> CreateAndSaveAsync(string email, string password)
    {
        var user = new User { Email = email };
        user.PasswordHash = _passwordHasher.HashPassword(user, password);
        await _context.AddAsync(user);
        await _context.SaveChangesAsync();
        return user;
    }

    [Fact]
    public async Task GetUserByEmail()
    {
        //Arrange
        var user = await CreateAndSaveAsync("Hkjasjkfajksfkj@mail.com", "HelloABc123£#");

        //Act
        User findUser = await _service.GetUserByEmail(email: user.Email);


        //Assert
        Assert.NotNull(findUser);
        Assert.Equal(user, findUser);
    }
}
