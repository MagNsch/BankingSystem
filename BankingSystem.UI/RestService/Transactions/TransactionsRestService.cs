using RestSharp;

namespace BankingSystem.UI.RestService.Transactions;

public class TransactionsRestService : ITransactionRestService
{
    private readonly RestClient _client;
    public TransactionsRestService()
    {
        _client = new RestClient("https://localhost:7168/api/accounts");
    }

    public async Task<bool> DepositToAccount(int accountId, decimal amount)
    {
        var request = new RestRequest($"deposit/{accountId}", Method.Put).AddJsonBody(new { Amount = amount });

        var response = await _client.ExecuteAsync<bool>(request);

        return response.IsSuccessful;
    }

    public async Task<bool> WithDrawFromAccount(int accountId, decimal amount)
    {
        var request = new RestRequest($"withdraw/{accountId}", Method.Put).AddJsonBody(new { Amount = amount });
        var response = await _client.ExecuteAsync<bool>(request);

        return response.IsSuccessful;
    }
}
