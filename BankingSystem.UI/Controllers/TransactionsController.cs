using BankingSystem.UI.RestService.Transactions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.UI.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly ITransactionRestService _transactionRestService;


        public TransactionsController(ITransactionRestService transactionRestService)
        {
            _transactionRestService = transactionRestService;
        }

        // GET: TransactionsController
        [Authorize]
        public async Task<ActionResult> GetAccountsTransactions(int accountId)
        {
            var accountTransactions = await _transactionRestService.GetAllTransactionsById(accountId);
            return View(accountTransactions);
        }

        // GET: TransactionsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TransactionsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TransactionsController/Create
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

        // GET: TransactionsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TransactionsController/Edit/5
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

        // GET: TransactionsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TransactionsController/Delete/5
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
}
