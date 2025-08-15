using BankingSystem.UI.Models;
using Microsoft.AspNetCore.Identity;

namespace BankingSystem.UI.Models;

public class User : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    public ICollection<Account> Accounts { get; set; }
}
