using BankingSystem.API.Models;
using BankingSystem.API.Services.UserServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
namespace BankingSystem.API.Controllers;

[Route("api/users")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<UsersController> _logger;

    public UsersController(IUserService userService, ILogger<UsersController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    // GET: api/<UsersController>
    [HttpGet("getbyemail/{email}")]
    public async Task<ActionResult<User>> GetUserByEmail(string email)
    {
        _logger.LogInformation("Request received to get user with email: {@email}", email);

        if (string.IsNullOrEmpty(email))
        {
            _logger.LogInformation("email is null or empty {@email}", email);
            return BadRequest(new { Message = "Email was not found" });
        }

        var user = await _userService.GetUserByEmail(email);

        if (user == null)
        {
            _logger.LogWarning("user with eamil: {@email} could not be found.", email);
            return NotFound(new { Message = "User with that eamil was not found" });
        }

        _logger.LogInformation("Successfully retrieved user with email: {@eamil}", email);
        return Ok(user);
    }
}
