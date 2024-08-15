using BankingSystem.API.Models;
using RestSharp;

namespace BankingSystem.UI.RestService;

public class RestServiceClient : IRestServiceClient
{
    private readonly RestClient _client;

    public RestServiceClient()
    {
        _client = new RestClient("https://localhost:7168/accounts");
    }
    public async Task<IEnumerable<Account>> GetAllAccounts()
    {
        var request = new RestRequest("", Method.Get);
        var response = await _client.ExecuteAsync<List<Account>>(request);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to get accounts: {response.StatusCode} - {response.ErrorMessage}");
        }
        
        return response.Data;
    }
}
