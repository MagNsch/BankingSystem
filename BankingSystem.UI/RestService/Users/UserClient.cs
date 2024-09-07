using BankingSystem.API.Models;
using BankingSystem.UI.Models;
using Newtonsoft.Json;
using RestSharp;

namespace BankingSystem.UI.RestService.Users;

public class UserClient : IUserClient
{

    private readonly RestClient _client;

    public UserClient()
    {
        _client = new RestClient("https://localhost:7168");
    }

    public async Task<RegisterModel> RegisterUser(RegisterModel model)
    {
        var request = new RestRequest("/register", Method.Post).AddJsonBody(model);
        var response = await _client.ExecuteAsync<RegisterModel>(request);

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
