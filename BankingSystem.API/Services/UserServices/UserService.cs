using BankingSystem.API.Models;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem.API.Services.UserServices;

public class UserService : IUserService
{

    private readonly ApplicationDbContext _context;

    public UserService(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<User> GetUserByEmail(string email)
    {
        var user = await _context.Users.SingleOrDefaultAsync(x => x.Email == email);

        if (user == null) { throw new Exception("User with that email was not found"); }

        return user;
    }
}
