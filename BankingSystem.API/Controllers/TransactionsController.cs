using BankingSystem.API.Models;
using BankingSystem.API.Services.CrudTransactions;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BankingSystem.API.Controllers;

[Route("api/transactions")]
[ApiController]
public class TransactionsController : ControllerBase
{
    private readonly ITransactionCRUD _transactionCRUD;

    public TransactionsController(ITransactionCRUD transactionsCrud)
    {

        _transactionCRUD = transactionsCrud;
    }
    // GET: api/<TransactionsController>
    [HttpGet("{accountId}")]
    public async Task<IEnumerable<AccountTransaction?>> GetAllTransactions(int accountId)
    {
        return await _transactionCRUD.GetAllTransaction(accountId);
    }

    //// GET api/<TransactionsController>/5
    //[HttpGet("{id}")]
    //public string Get(int id)
    //{
    //    return "value";
    //}

    // POST api/<TransactionsController>
    //[HttpPost]
    //public void Post([FromBody] string value)
    //{
    //}

    //// PUT api/<TransactionsController>/5
    //[HttpPut("{id}")]
    //public void Put(int id, [FromBody] string value)
    //{
    //}

    //// DELETE api/<TransactionsController>/5
    //[HttpDelete("{id}")]
    //public void Delete(int id)
    //{
    //}
}
