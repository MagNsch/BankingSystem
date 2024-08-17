using BankingSystem.API.Models;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem.API.Services;

public interface ITransactionCRUD
{
    Task<IEnumerable<AccountTransaction?>> GetAllTransaction(int accountId);
    

}
