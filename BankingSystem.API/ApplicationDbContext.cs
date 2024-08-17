using BankingSystem.API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace BankingSystem.API;

public class ApplicationDbContext : IdentityDbContext<User>
{
    public DbSet<Account> Accounts { get; set; }

    public DbSet<AccountTransaction> Transactions { get; set; }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=application.db");

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<Account>().Property(a => a.Balance).HasDefaultValue(0);
        modelBuilder.Entity<Account>()
           .HasOne(a => a.User)
           .WithMany(u => u.Accounts)
           .HasForeignKey(a => a.UserId);

        modelBuilder.Entity<AccountTransaction>().HasKey(a => a.TransactionId);
        modelBuilder.Entity<AccountTransaction>()
            .HasOne(a => a.Account)
            .WithMany(u => u.Transactions)
            .HasForeignKey(a => a.AccountId);
        base.OnModelCreating(modelBuilder);
    }
}


