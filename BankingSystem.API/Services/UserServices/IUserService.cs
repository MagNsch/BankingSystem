using BankingSystem.API.Models;
using Microsoft.AspNetCore.Identity;

namespace BankingSystem.API.Services.UserServices;

public interface IUserService
{
    Task<User> GetUserByEmail(string email);
}
