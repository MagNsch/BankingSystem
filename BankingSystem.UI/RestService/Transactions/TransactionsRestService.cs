using BankingSystem.API.Models;
using RestSharp;

namespace BankingSystem.UI.RestService.Transactions;

public class TransactionsRestService : ITransactionRestService
{
    private readonly RestClient _client;
    private readonly RestClient _transactionClient;
    public TransactionsRestService()
    {
        _client = new RestClient("https://localhost:7168/api/accounts");
        _transactionClient = new RestClient("https://localhost:7168/api/transactions");
    }

    public async Task<IEnumerable<AccountTransaction>> AccountTransactions(int accountId)
    {
        var request = new RestRequest($"{accountId}", Method.Get);
        var response = await _transactionClient.ExecuteAsync<IEnumerable<AccountTransaction>>(request);

        return response.Data;   
    }

    public async Task<bool> DepositToAccount(int accountId, decimal amount)
    {
        var request = new RestRequest($"deposit/{accountId}", Method.Put).AddJsonBody(new { Amount = amount });
        var response = await _client.ExecuteAsync(request);
        
        return response.IsSuccessful;
    }

    public async Task<bool> WithDrawFromAccount(int accountId, decimal amount)
    {
        var request = new RestRequest($"withdraw/{accountId}", Method.Put).AddJsonBody(new { Amount = amount });
        var response = await _client.ExecuteAsync(request);
        
        return response.IsSuccessful;
    }
}
