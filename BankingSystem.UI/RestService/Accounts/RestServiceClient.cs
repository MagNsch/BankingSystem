using BankingSystem.API.Models;
using RestSharp;

namespace BankingSystem.UI.RestService.Accounts;

public class RestServiceClient : IRestServiceClient
{
    private readonly RestClient _client;

    public RestServiceClient()
    {
        _client = new RestClient("https://localhost:7168/api/accounts");
    }
    public async Task<IEnumerable<Account>> GetAllAccounts(string userId)
    {
        var request = new RestRequest($"getall/{userId}", Method.Get);
        var response = await _client.ExecuteAsync<List<Account>>(request);

        if (!response.IsSuccessStatusCode)
        {
            var errorMessage = response.ErrorMessage ?? "No error message provided";
            throw new Exception($"Failed to get accounts: {response.StatusCode} - {errorMessage}");
        }

        if (response.Data == null)
        {
            throw new Exception("Failed to get accounts: response data is null.");
        }

        return response.Data;
    }

    public async Task<Account> GetAccountById(int id)
    {
        var request = new RestRequest($"{id}", Method.Get);
        var response = await _client.ExecuteAsync<Account>(request);

        string errorMessage = "Failed to get account";
        Exceptions(response, errorMessage);

        return response.Data;
    }

    public async Task<Account> CreateAccount(Account account)
    {
        var request = new RestRequest("newaccount", Method.Post).AddJsonBody(account);
        var response = await _client.ExecuteAsync<Account>(request);

        string errorMessage = "Failed to create account";
        Exceptions(response, errorMessage);

        return response.Data;
    }

    public async Task<bool> DeleteAccount(int id)
    {
        var request = new RestRequest($"{id}", Method.Delete);
        var response = await _client.ExecuteAsync(request);
        return response.IsSuccessStatusCode;
    }


    //Helper methods

    private static void Exceptions(RestResponse<Account> response, string errorMessageForAccount)
    {
        if (!response.IsSuccessStatusCode)
        {
            var errorMessage = response.ErrorMessage ?? "No error message provieded";
            throw new Exception($"{errorMessageForAccount}: {response.StatusCode} - {errorMessage}");
        }
        if (response.Data is null)
        {
            throw new Exception($"{errorMessageForAccount}: response data is null.");
        }
    }
}
