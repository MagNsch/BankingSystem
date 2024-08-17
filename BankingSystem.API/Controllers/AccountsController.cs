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
    public async Task<ActionResult<Account>> DepositAccount(int accountId, decimal amount)
    {
        if (amount <= 0)
        {
            return BadRequest("The deposit amount must be greater than zero.");
        }

        var result = await _transactionService.DepositAccount(accountId, amount);

        if (!result)
        {
            return NotFound("Account could not be found!");
        }
        return Ok("Deposit was succesful");
    }

    [HttpPut("withdraw/{accountId}")]
    public async Task<ActionResult<Account>> WithDrawFromAccount(int accountId, decimal amount)
    {
        var result = await _transactionService.WithDrawFromAccount(accountId, amount);

        if (!result)
        {
            return NotFound("The withdrawed amount must be below the accounts balance");
        }
        return Ok("Withdraw was succesful");
    }

    // GET: api/Accounts
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Account>>> GetAccounts()
    {
        var user = _context.Users.FirstOrDefault(u => u.Id == User.FindFirstValue(ClaimTypes.NameIdentifier));

        if (user.Id == null || string.IsNullOrEmpty(user.Id))
        {
            return Unauthorized();
        }

        return await _context.Accounts.Where(a => a.UserId == user.Id).ToListAsync();
    }

    // GET: api/Accounts/5
    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<Account>> GetAccount(int id)
    {
        var user = _context.Users.FirstOrDefault(u => u.Id == User.FindFirstValue(ClaimTypes.NameIdentifier));

        if (string.IsNullOrEmpty(user.Id))
        {
            return Unauthorized();
        }

        var account = await _context.Accounts.FirstOrDefaultAsync(a => a.AccountId == id && a.UserId == user.Id);

        if (account == null)
        {
            return NotFound();
        }

        return account;
    }

    // POST: api/Accounts
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Account>> PostAccount(Account account)
    {
        var user = _context.Users.FirstOrDefault(u => u.Id == User.FindFirstValue(ClaimTypes.NameIdentifier));
        if (string.IsNullOrEmpty(user.Id))
        {
            return Unauthorized();
        }
        account.UserId = user.Id;

        _context.Accounts.Add(account);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetAccount", new { id = account.AccountId }, account);
    }

    // DELETE: api/Accounts/5
    [Authorize]
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
