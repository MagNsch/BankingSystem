using BankingSystem.API.Models;
using BankingSystem.UI.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using BankingSystem.API;
using Microsoft.AspNetCore.Identity;
using BankingSystem.UI.RestService.Users;
using Microsoft.AspNetCore.Authorization;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem.UI.Controllers;

public class UsersController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IUserClient _userClient;

    public UsersController(ApplicationDbContext context, IPasswordHasher<User> passwordHasher, IUserClient userClient)
    {
        _passwordHasher = passwordHasher;
        _context = context;
        _userClient = userClient;
    }

    // GET: UsersController
    public ActionResult Login() => View();

    public IActionResult AccessDenied() => View();

    public IActionResult RegisterUser() => View();

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> RegisterUser(RegisterModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {

            if (!IsValidPassword(model.Password))
            {
                ModelState.AddModelError("", "Password must be at least 6 characters long, include at least one uppercase letter, one lowercase letter, and one special character.");
                return View(model);
            }

            var newUser = await _userClient.RegisterUser(model);


            var userAllreadyExist = await _userClient.GetUserByEmail(model.Email);
            if (userAllreadyExist is not null)
            {
                ModelState.AddModelError("", "User with that Email allready exist");
            }
            
            var user = await _userClient.GetUserByEmail(model.Email);
            if (user is null)
            {
                ModelState.AddModelError("", "Could not create user");
                return View(model);
            }
            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, model.Password);
            if (result is PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError(string.Empty, "Invalid Password");
                return View(model);
            }

            await _context.SaveChangesAsync();
            await SignIn(user);

            return RedirectToAction("Index", "Accounts");
        }
        catch (Exception)
        {

            ModelState.AddModelError("", "An error occurred while processing your request. Please try again.");
            return View(model);
        }
    }

    private static bool IsValidPassword(string password)
    {
        if (password.Length < 6)
            return false;

        // Regulært udtryk for at sikre mindst én stor bogstav, én lille bogstav, og én speciel karakter
        var hasUpperChar = new Regex(@"[A-Z]");
        var hasLowerChar = new Regex(@"[a-z]");
        var hasSpecialChar = new Regex(@"[!@#$%^&*(),.?"":{}|<>]");

        return hasUpperChar.IsMatch(password) &&
               hasLowerChar.IsMatch(password) &&
               hasSpecialChar.IsMatch(password);
    }


    public async Task<IActionResult> LogOut()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        TempData["Message"] = "You are now logged out...";
        return RedirectToAction("Index", "");
    }

    [HttpPost]
    public async Task<IActionResult> Login([FromForm] LoginModel loginModel, [FromQuery] string returnUrl)
    {
        User? user = await _userClient.GetUserByEmail(loginModel.Email);
        if (user is null) { return RedirectToAction("RegisterUser", "users"); }

        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginModel.Password);
        if (result is PasswordVerificationResult.Failed)
        {
            ModelState.AddModelError(string.Empty, "Invalid Password");
            return View(loginModel);
        }
        else
        {
            if (user is not null) { await SignIn(user); }
        }

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
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

        TempData["message"] = $"You are logged in as {claimsIdentity.Name}";
    }
}
