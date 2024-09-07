using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace BankingSystem.UI.Validation
{
    public class PasswordValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            var password = value as string;

            if(string.IsNullOrEmpty(password))
            {
                return false;
            }

            if(password.Length < 6)
            {
                return false;
            }

            var hasUpperChar = new Regex(@"[A-Z]");
            var hasLowerChar = new Regex(@"[a-z]");
            var hasSpecialChar = new Regex(@"[!@#$%^&*(),.?"":{}|<>]");

            return hasUpperChar.IsMatch(password) &&
                   hasLowerChar.IsMatch(password) &&
                   hasSpecialChar.IsMatch(password);
        }
    }
}
