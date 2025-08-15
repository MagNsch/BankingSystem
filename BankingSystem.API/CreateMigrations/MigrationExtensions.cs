using Microsoft.EntityFrameworkCore;

namespace BankingSystem.API.CreateMigrations;

public static class MigrationExtensions
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        using ApplicationDbContext context = scope.ServiceProvider.GetService<ApplicationDbContext>();

        context.Database.Migrate();
    }
}
