using BankingSystem.API.Models;
using BankingSystem.UI.Models;
using Microsoft.AspNetCore.Identity;
using RestSharp;

namespace BankingSystem.UI.RestService.Users;

public class UserClient : IUserClient
{

    private readonly RestClient _client;
    private readonly RestClient _client2;

    public UserClient()
    {
        _client2 = new RestClient("https://localhost:7168");
        _client = new RestClient("https://localhost:7168/api/users");
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        var request = new RestRequest($"getbyemail/{email}", Method.Get);
        var response = await _client.ExecuteAsync<User>(request);

        if (response.IsSuccessful)
        {
            return response.Data;
        }
        return null;
    }

    public async Task<RegisterModel> RegisterUser(RegisterModel model)
    {
        var request = new RestRequest("/register", Method.Post).AddJsonBody(model);
        var response = await _client2.ExecuteAsync<RegisterModel>(request);

        if (response.IsSuccessful && response.Data != null)
        {
            return response.Data;
        }
        else
        {
            return null;
        }
    }
}
