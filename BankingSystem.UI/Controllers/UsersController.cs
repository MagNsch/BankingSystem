using BankingSystem.API.Models;
using BankingSystem.UI.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using BankingSystem.API;

namespace BankingSystem.UI.Controllers;

public class UsersController : Controller
{
    private readonly ApplicationDbContext _context;

    public UsersController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: UsersController
    public ActionResult Login() => View();

    public IActionResult AccessDenied() => View();
    public async Task<IActionResult> LogOut()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        TempData["Message"] = "You are now logged out...";
        return RedirectToAction("Index", "");
    }

    [HttpPost]
    public async Task<IActionResult> Login([FromForm] LoginModel loginModel, [FromQuery] string returnUrl)
    {
        User? user = _context.Users.FirstOrDefault(u => u.Email == loginModel.Email);
        if (user == null) { return RedirectToAction("RegisterUser", "accounts"); }

        if (user != null) { await SignIn(user); }
        if (string.IsNullOrEmpty(returnUrl))
            //Registring redirect
            return RedirectToAction("Index", "Home");

        else
            return Redirect(returnUrl);
    }

    private async Task SignIn(User user)
    {
        var claims = new List<Claim>
        {
            new Claim("id",user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            //new Claim(ClaimTypes.Name, user.FirstName)
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

        TempData["message"] = $"You are logged in as {claimsIdentity.Name}";
    }












    // GET: UsersController/Details/5
    public ActionResult Details(int id)
    {
        return View();
    }

    // GET: UsersController/Create
    public ActionResult Create()
    {
        return View();
    }

    // POST: UsersController/Create
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


}
