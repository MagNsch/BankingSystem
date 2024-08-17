namespace BankingSystem.API.Models;

public class AccountTransaction
{
    public int TransactionId { get; set; }

    public decimal TransferedAmmount { get; set; }

    public DateTime? CreatedDate { get; set; } 
    public Account? Account { get; set; }
    public int AccountId { get; set; }
}
