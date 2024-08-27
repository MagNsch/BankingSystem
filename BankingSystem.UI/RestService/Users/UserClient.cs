using BankingSystem.API.Models;
using RestSharp;
using static System.Net.WebRequestMethods;

namespace BankingSystem.UI.RestService.Users;

public class UserClient : IUserClient
{

    private readonly RestClient _client;

    public UserClient(RestClient client)
    {
        
        _client = new RestClient("https://localhost:7168");
    }

    public async Task<User> RegisterUser(User user)
    {
        var request = new RestRequest($"register", Method.Post).AddJsonBody(user);
        var response = await _client.ExecuteAsync<User>(request);
        Console.WriteLine(user.Email);
        return response.Data;
        
    }
}
