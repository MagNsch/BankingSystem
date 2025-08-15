namespace BankingSystem.API.Services;

using BankingSystem.API.Models;
using Microsoft.AspNetCore.Identity;
using System.Net.Mail;
using System.Threading.Tasks;

public class EmailSender : IEmailSender<User>
{
    public Task SendConfirmationLinkAsync(User user, string email, string confirmationLink)
    {
        // enten implementer mail-logik eller bare returner CompletedTask
        return Task.CompletedTask;
    }

    public Task SendPasswordResetCodeAsync(User user, string email, string resetCode)
    {
        return Task.CompletedTask;
    }

    public Task SendPasswordResetLinkAsync(User user, string email, string resetLink)
    {
        return Task.CompletedTask;
    }

}

