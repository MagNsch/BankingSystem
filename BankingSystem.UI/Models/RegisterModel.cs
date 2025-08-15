using BankingSystem.UI.Validation;
using System.ComponentModel.DataAnnotations;

namespace BankingSystem.UI.Models
{
    public class RegisterModel()
    {
        [EmailAddress]
        public required string Email { get; set; }
    
        [PasswordValidation(ErrorMessage = "Password must be at least 6 characters long, include at least one uppercase letter, one lowercase letter, and one special character.")]
        public required string Password { get; set; }
    }
}
