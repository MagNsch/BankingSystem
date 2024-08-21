using Microsoft.AspNetCore.Identity;

namespace BankingSystem.API.Models;

public class User : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    public ICollection<Account> Accounts { get; set; }
}
