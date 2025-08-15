using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BankingSystem.API.Models;

public class Account
{
    [Key]
    public int AccountId { get; set; }

    public string AccountName { get; set; }

    public decimal Balance { get; set; } = 0;

    public string UserId { get; set; }

    public AccountType AccountType { get; set; } 

    [JsonIgnore]
    public User? User { get; set; }
    [JsonIgnore]
    public ICollection<AccountTransaction?> Transactions { get; set; } = [];

    public Account(string accountName, decimal balance, string userId, AccountType accountType)
    {
        AccountName = accountName;
        Balance = balance;
        UserId = userId;
        AccountType = accountType;
    }

    public Account()
    {
        
    }
}