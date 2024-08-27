using BankingSystem.API.Models;
using BankingSystem.UI.Models;
using BankingSystem.UI.RestService.Accounts;
using BankingSystem.UI.RestService.Transactions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.UI.Controllers;

public class AccountsController : Controller
{
    private readonly IRestServiceClient _restClient;
    private readonly ITransactionRestService _transactionRestService;

    public AccountsController(IRestServiceClient restClient, ITransactionRestService transactionRestService)
    {
        _restClient = restClient;
        _transactionRestService = transactionRestService;
    }

    [Authorize]
    public async Task<IActionResult> DepositAmount(int id)
    {
        var account = await _restClient.GetAccountById(id);
        return View(account);
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DepositAmount(int id, decimal amount)
    {
        if (ModelState.IsValid)
        {
            ViewBag.Amount = amount;
            bool added = await _transactionRestService.DepositToAccount(id, amount);
            if (!added)
            {
                ModelState.AddModelError("", "Deposit did not succed");
                return View();
            }
            return RedirectToAction("index");

        }
        return View();
    }

    [Authorize]
    public async Task<IActionResult> WithdrawAmount(int id)
    {
        var account = await _restClient.GetAccountById(id);
        return View(account);
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> WithdrawAmount(int id, decimal amount)
    {
        var account = await _restClient.GetAccountById(id);

        if (ModelState.IsValid)
        {
            ViewBag.Amount = amount;

            if(amount <= 0)
            {
                ModelState.AddModelError("", "The amount must be greater than zero");
            }

            if(amount > account.Balance)
            {
                ModelState.AddModelError("", "Insufficient Funds");
                return View();
            }
            
            bool succes = await _transactionRestService.WithDrawFromAccount(id, amount);
            if (!succes)
            {
                ModelState.AddModelError("", "Failed to withdraw from account. Please try again.");
                return View();
            }
            return RedirectToAction("index");
        }
        return View();
    }

    // GET: AccountsController
    [Authorize]
    public async Task<IActionResult> Index()
    {
        string userId = User.FindFirst("id")?.Value;
        if (userId is null)
        {
            return RedirectToAction("Register");
        }
        var usersAccounts = await _restClient.GetAllAccounts(userId);
        return View(usersAccounts);
    }

    // GET: AccountsController/Details/5
    public async Task<ActionResult> Details(int id)
    {
        var account = await _restClient.GetAccountById(id);
        return View(account);
    }

    // GET: AccountsController/Create
    public ActionResult CreateAccount() => View();

    // POST: AccountsController/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> CreateAccount(Account account)
    {

        try
        {
            string userId = User.FindFirst("id")?.Value;
            if (userId is null)
            {
                return RedirectToAction("Register");
            }
            account.UserId = userId;
            var accountToCreate = await _restClient.CreateAccount(account);

            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View(account);
        }
    }

    // GET: AccountsController/Edit/5
    public ActionResult Edit(int id)
    {
        return View();
    }

    // POST: AccountsController/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(int id, IFormCollection collection)
    {
        try
        {
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View();
        }
    }

    // GET: AccountsController/Delete/5
    public async Task<ActionResult> Delete(int id)
    {
        var account = await _restClient.GetAccountById(id);
        return View(account);
    }

    // POST: AccountsController/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Delete(int id, IFormCollection collection)
    {
        try
        {
            //var accountToGet = await _restClient.GetAccountById(id);

            var accountToDelete = await _restClient.DeleteAccount(id);

            return RedirectToAction("index");
        }
        catch
        {
            return View();
        }
    }
}
