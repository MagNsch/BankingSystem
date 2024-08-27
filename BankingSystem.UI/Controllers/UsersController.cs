﻿using BankingSystem.API.Models;
using BankingSystem.UI.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using BankingSystem.API;
using Microsoft.AspNetCore.Identity;

namespace BankingSystem.UI.Controllers;

public class UsersController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IPasswordHasher<User> _passwordHasher;

    public UsersController(ApplicationDbContext context, IPasswordHasher<User> passwordHasher)
    {
        _passwordHasher = passwordHasher;
        _context = context;
    }

    // GET: UsersController
    public ActionResult Login() => View();

    public IActionResult AccessDenied() => View();

    //public async Task<IActionResult> RegisterUser(RegisterModel model)
    //{
    //    try
    //    {
    //        var user = new User();
    //        user.Email = model.Email;
    //        user.Pass


    //        await _context.Users.AddAsync(user);
    //    }
    //    catch (Exception)
    //    {

    //        throw;
    //    }
    //    return View(model);
    //}


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

        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginModel.Password);
        if (result == PasswordVerificationResult.Failed)
        {
            ModelState.AddModelError(string.Empty, "Invalid Password");
            return View(loginModel);
        }
        else
        {
            if (user != null) { await SignIn(user); }
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
