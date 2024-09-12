using BankingSystem.API.Models;
using BankingSystem.API.Services.AccountServices;
using BankingSystem.API.Services.TransactionServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem.API.Controllers;

[Route("api/accounts")]
[ApiController]
public class AccountsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ITransactionService _transactionService;
    private readonly IAccountService _accountService;

    public AccountsController(ApplicationDbContext context, ITransactionService transactionService, IAccountService accountService)
    {
        _context = context;
        _transactionService = transactionService;
        _accountService = accountService;
    }

    [HttpPut("deposit/{accountId}")]
    public async Task<ActionResult<bool>> DepositAccount(int accountId, [FromBody] TransferRequest request)
    {
        if (request.Amount < 0)
        {
            return BadRequest("The deposit amount must be greater than zero.");
        }

        var result = await _transactionService.DepositAccount(accountId, request.Amount);

        if (!result)
        {
            return NotFound("Account could not be found!");
        }
        return Ok("Deposit was succesful");
    }

    [HttpPut("withdraw/{accountId}")]
    public async Task<ActionResult<bool>> WithDrawFromAccount(int accountId, [FromBody] TransferRequest request)
    {
        var result = await _transactionService.WithDrawFromAccount(accountId, request.Amount);

        if (!result)
        {
            return NotFound("The withdrawed amount must be below the accounts balance");
        }
        return Ok("Withdraw was succesful");
    }

    // GET: api/Accounts
    [HttpGet("getall/{userId}")]
    public async Task<ActionResult<IEnumerable<Account?>>> GetAccounts(string userId)
    {
        var accounts = await _accountService.GetAllAccounts(userId);

        return Ok(accounts);
    }

    // GET: api/Accounts/5

    [HttpGet("{id}")]
    public async Task<ActionResult<Account>> GetAccount(int id)
    {
        var account = await _accountService.GetAccount(id);

        if (account == null)
        {
            return NotFound("account with that id could not be found");
        }

        return account;
    }

    // POST: api/Accounts
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost("newaccount")]
    public async Task<ActionResult<Account>> PostAccount(Account account)
    {
         await _accountService.CreateAccount(account);

        return CreatedAtAction("GetAccount", new { id = account.AccountId }, account);
    }

    // DELETE: api/Accounts/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAccount(int id)
    {
        await _accountService.DeleteAccount(id);

        return NoContent();
    }
}
