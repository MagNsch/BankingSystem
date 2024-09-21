using BankingSystem.API.Models;
using BankingSystem.API.Services.AccountServices;
using BankingSystem.API.Services.TransactionServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Serilog;
using System.Security.Principal;

namespace BankingSystem.API.Controllers;

[Route("api/accounts")]
[ApiController]
public class AccountsController : ControllerBase
{
    private readonly ILogger<AccountsController> _logger;
    private readonly ITransactionService _transactionService;
    private readonly IAccountService _accountService;

    public AccountsController(ILogger<AccountsController> logger, ITransactionService transactionService, IAccountService accountService)
    {
        _logger = logger;
        _transactionService = transactionService;
        _accountService = accountService;
    }

    [HttpPut("deposit/{accountId}")]
    public async Task<ActionResult<bool>> DepositAccount(int accountId, [FromBody] TransferRequest request)
    {
        _logger.LogInformation("Starting deposit for AccountId: {@AccountId} with amount: {@Amount}", accountId, request.Amount);
        if (request.Amount < 0)
        {
            _logger.LogWarning("Deposit attempt with negative amount: {@Amount} for AccountId: {@AccountId}", request.Amount, accountId);
            return BadRequest(new { Message = "The deposit amount must be greater than zero." });
           
        }

        var result = await _transactionService.DepositAccount(accountId, request.Amount);

        if (!result)
        {
            _logger.LogWarning("Deposit failed. Account with AccountId: {@AccountId} not found.", accountId);
            return NotFound(new { Message = "Account could not be found!" });
            
        }

        _logger.LogInformation("Deposit successful for AccountId: {@AccountId} with amount: {@Amount}", accountId, request.Amount);
        return Ok(new{ Message = "Deposit was successful"});
        
    }

    [HttpPut("withdraw/{accountId}")]
    public async Task<ActionResult<bool>> WithDrawFromAccount(int accountId, [FromBody] TransferRequest request)
    {
        _logger.LogInformation("Starting withdrawing from account with AccountId: {@AccountId} with amount: {@Amount}", accountId, request.Amount);
        var result = await _transactionService.WithDrawFromAccount(accountId, request.Amount);

        if (!result)
        {
            return NotFound(new { Message = "The withdrawed amount must be below the accounts balance" });
            
        }
        return Ok(new { Message = "Withdraw was succesful" });
        
    }

    // GET: api/Accounts
    [HttpGet("getall/{userId}")]
    public async Task<ActionResult<IEnumerable<Account?>>> GetAccounts(string userId)
    {
        _logger.LogInformation("Retrieving accounts for UserId: {@UserId}", userId);

        if (string.IsNullOrEmpty(userId))
        {
            _logger.LogWarning("Invalid UserId: {@UserId}. UserId cannot be null or empty.", userId);
            return BadRequest(new { Message = "UserId cannot be null or empty." });
            
        }

        var accounts = await _accountService.GetAllAccounts(userId);

        _logger.LogInformation("{AccountCount} accounts found for UserId: {@UserId}", accounts.Count(), userId);
        return Ok(accounts);
    }

    // GET: api/Accounts/5

    [HttpGet("{id}")]
    public async Task<ActionResult<Account>> GetAccount(int id)
    {
        _logger.LogInformation("Request received to get account with id: {@id}", id);

        if (id <= 0)
        {
            _logger.LogWarning("Invalid accountId: {@id}. AccountId must be greater than 0.", id);
            return BadRequest(new { Message = "Account ID must be greater than 0." });
            
        }
        var account = await _accountService.GetAccount(id);

        if (account == null)
        {
            _logger.LogWarning("Account with id: {@id} could not be found.", id);
            return NotFound(new { Message = "Account with that id could not be found." });
        }

        _logger.LogInformation("Successfully retrieved account with id: {@id}", id);
        return Ok(account);
    }

    // POST: api/Accounts
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost("newaccount")]
    public async Task<ActionResult<Account>> PostAccount(Account account)
    {
        _logger.LogInformation("Request received to create account: {@account}", account);
        await _accountService.CreateAccount(account);

        if (account is null)
        {
            _logger.LogWarning("Account could not be created");
            return BadRequest(new { Message = "Account was null" });
        }

        _logger.LogInformation("Successfully created account with: {@id}", account.AccountId);
        return CreatedAtAction("GetAccount", new { id = account.AccountId }, account);
    }

    // DELETE: api/Accounts/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAccountAsync(int id)
    {
        _logger.LogInformation("Request received to delete account with ID: {id}", id);

        // Validate account ID
        if (id <= 0)
        {
            _logger.LogWarning("Invalid accountId: {@id}. AccountId must be greater than 0.", id);
            return BadRequest(new { Message = "Invalid account ID. It must be greater than 0." });
        }

        try
        {
            // Try to delete the account
            var isDeleted = await _accountService.DeleteAccount(id);

            if (!isDeleted)
            {
                _logger.LogWarning("Account with ID {id} could not be found for deletion.", id);
                return NotFound(new { Message = $"Account with ID {id} does not exist." });
            }

            // Log successful deletion
            _logger.LogInformation("Successfully deleted account with ID {id}.", id);
            return NoContent();
        }
        catch (Exception ex)
        {
            // Log the exception
            _logger.LogError(ex, "An error occurred while trying to delete account with ID {id}.", id);
            return StatusCode(500, new { Message = "An error occurred while processing your request." });
        }
    }
}
