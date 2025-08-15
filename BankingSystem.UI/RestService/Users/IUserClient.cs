using BankingSystem.UI.Models;

namespace BankingSystem.UI.RestService.Users;

public interface IUserClient
{
    Task<RegisterModel> RegisterUser(RegisterModel model);

    Task<User?> GetUserByEmail(string email);
}
