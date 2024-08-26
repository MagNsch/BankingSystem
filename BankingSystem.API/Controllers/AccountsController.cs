using BankingSystem.API.Models;
using BankingSystem.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BankingSystem.API.Controllers;

[Route("api/accounts")]
[ApiController]
public class AccountsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ITransactionService _transactionService;

    public AccountsController(ApplicationDbContext context, ITransactionService transactionService)
    {
        _context = context;
        _transactionService = transactionService;
    }

    [HttpPut("deposit/{accountId}")]
    public async Task<ActionResult<bool>> DepositAccount(int accountId, [FromBody] TransferRequest request)
    {
        if (request.Amount <= 0)
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
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

        var accounts = await _context.Accounts.Where(a => a.UserId == user.Id).ToListAsync();

        return Ok(accounts);
    }

    // GET: api/Accounts/5

    [HttpGet("{id}")]
    public async Task<ActionResult<Account>> GetAccount(int id)
    {
        var account = await _context.Accounts.FirstOrDefaultAsync(a => a.AccountId == id);

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
        var user = _context.Users.FirstOrDefault(u => u.Id == account.UserId);
        if (string.IsNullOrEmpty(user.Id))
        {
            return Unauthorized();
        }
        _context.Accounts.Add(account);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetAccount", new { id = account.AccountId }, account);
    }

    // DELETE: api/Accounts/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAccount(int id)
    {
        var account = await _context.Accounts.FindAsync(id);
        
        if (account == null)
        {
            return NotFound();
        }

        _context.Accounts.Remove(account);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
