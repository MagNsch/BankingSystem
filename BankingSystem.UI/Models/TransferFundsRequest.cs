namespace BankingSystem.UI.Models;

public class TransferFundsRequest
{
    public int FromAccountId { get; set; }
    public int ToAccountId { get; set; }
    public decimal Amount { get; set; }
}
