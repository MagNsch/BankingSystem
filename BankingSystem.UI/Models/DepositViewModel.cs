namespace BankingSystem.UI.Models;

public class DepositViewModel
{
    public int AccountId { get; set; }
    public decimal Amount { get; set; }
    public decimal? Balance { get; set; } 
    public string? Message { get; set; }
}
