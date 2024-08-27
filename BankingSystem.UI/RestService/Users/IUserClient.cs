using BankingSystem.API.Models;
using RestSharp;

namespace BankingSystem.UI.RestService.Users;

public interface IUserClient
{
    Task<User> RegisterUser(User user);

    
}
