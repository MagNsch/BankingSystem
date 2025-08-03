using BankingSystem.API.Models;
using BankingSystem.API.Services.CrudTransactions;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BankingSystem.API.Controllers;

[Route("api/transactions")]
[ApiController]
public class TransactionsController(ITransactionCRUD transactionsCrud, ILogger<ITransactionCRUD> logger) : ControllerBase
{
    private readonly ITransactionCRUD _transactionCRUD = transactionsCrud;
    private readonly ILogger<ITransactionCRUD> _logger = logger;

    // GET: api/<TransactionsController>
    [HttpGet("{accountId}")]
    public async Task<ActionResult<IEnumerable<AccountTransaction?>>> GetAllTransactions(int accountId)
    {
        _logger.LogInformation("Request received to retrieve all transactions for account with ID: {accountId}", accountId);
        if (accountId <= 0)
        {
            _logger.LogWarning("Invalid accountId: {accountId}. AccountId must be greater than 0.", accountId);
            return BadRequest(new {Message = "Account ID must be greater than 0."});
        }
        try
        {
            _logger.LogInformation("Fetching transactions for account with ID: {accountId}", accountId);
            var transactions = await _transactionCRUD.GetAllTransaction(accountId);
            return Ok(transactions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving transactions for account with ID: {accountId}", accountId);
            throw;
        }
    }
}
