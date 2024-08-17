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

    [JsonIgnore]
    public User? User { get; set; }
    [JsonIgnore]
    public ICollection<AccountTransaction?> Transactions { get; set; } = new List<AccountTransaction?>();

}
