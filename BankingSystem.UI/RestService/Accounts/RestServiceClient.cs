using BankingSystem.UI.Models;
using RestSharp;

namespace BankingSystem.UI.RestService.Accounts;

public class RestServiceClient : IRestServiceClient
{
    private readonly RestClient _client;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public RestServiceClient(IHttpContextAccessor httpContextAccessor)
    {
        _client = new RestClient("https://localhost:7168/api/accounts");
        _httpContextAccessor = httpContextAccessor;
    }
    public async Task<IEnumerable<Account>> GetAllAccounts()
    {

        var request = new RestRequest($"getall", Method.Get);
        AddAuthCookieToRequest(request);
        var response = await _client.ExecuteAsync<List<Account>>(request);

        if (!response.IsSuccessStatusCode)
        {
            var errorMessage = response.ErrorMessage ?? "Could not get all accounts";
            throw new Exception($"Failed to get accounts: {response.StatusCode} - {errorMessage}");
        }

        if (response.Data == null)
        {
            throw new Exception("Failed to get accounts: response data is null.");
        }

        return response.Data;
    }

    private void AddAuthCookieToRequest(RestRequest request)
    {
        var cookie = _httpContextAccessor.HttpContext?.Request.Cookies[".AspNetCore.Identity.Application"];
        if (!string.IsNullOrEmpty(cookie))
        {
            request.AddHeader("Cookie", $".AspNetCore.Identity.Application={cookie}");
        }
    }

    public async Task<Account> GetAccountById(int id)
    {
        var request = new RestRequest($"{id}", Method.Get);
        AddAuthCookieToRequest(request);
        var response = await _client.ExecuteAsync<Account>(request);

        string errorMessage = "Failed to get account";
        Exceptions(response, errorMessage);

        return response.Data;
    }

    public async Task<Account> CreateAccount(Account account)
    {
        var request = new RestRequest("newaccount", Method.Post).AddJsonBody(account);
        AddAuthCookieToRequest(request);
        var response = await _client.ExecuteAsync<Account>(request);

        string errorMessage = "Failed to create account";
        Exceptions(response, errorMessage);

        return response.Data;
    }

    public async Task<bool> DeleteAccount(int id)
    {
        var request = new RestRequest($"{id}", Method.Delete);
        AddAuthCookieToRequest(request);
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
