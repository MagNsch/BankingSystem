using BankingSystem.API.Models;
using BankingSystem.UI.Models;
using RestSharp;

namespace BankingSystem.UI.RestService.Users;

public interface IUserClient
{
    Task<RegisterModel> RegisterUser(RegisterModel model);

    
}
