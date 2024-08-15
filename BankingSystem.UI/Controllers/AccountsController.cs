using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.UI.Controllers;

public class AccountsController : Controller
{
    // GET: AccountsController
    public ActionResult Index()
    {
        return View();
    }

    // GET: AccountsController/Details/5
    public ActionResult Details(int id)
    {
        return View();
    }

    // GET: AccountsController/Create
    public ActionResult Create()
    {
        return View();
    }

    // POST: AccountsController/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create(IFormCollection collection)
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
