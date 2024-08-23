using BankingSystem.API.Models;
using BankingSystem.UI.RestService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using System.Security.Claims;

namespace BankingSystem.UI.Controllers;

public class AccountsController : Controller
{
    private readonly IRestServiceClient _restClient;

    public AccountsController(IRestServiceClient restClient)
    {
        _restClient = restClient;
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
    public ActionResult CreateAccount()
    {

        return View();
    }

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
    public ActionResult Delete(int id)
    {
        return View();
    }

    // POST: AccountsController/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Delete(int id, IFormCollection collection)
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
}
