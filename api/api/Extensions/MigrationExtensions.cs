using api.Data;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Extensions
{
    public static class MigrationExtensions
    {
        public static void ApplyMigrations(this IApplicationBuilder applicationBuilder)
        {
            Console.WriteLine("Applying migrations...");
            using IServiceScope scope = applicationBuilder.ApplicationServices.CreateScope();
            using BankingDbContext dbContext = scope.ServiceProvider.GetRequiredService<BankingDbContext>();

            dbContext.Database.Migrate();
        }

        public static void SeedDataBase(this IApplicationBuilder applicationBuilder)
        {
            Console.WriteLine("Seeding database...");
            using IServiceScope scope = applicationBuilder.ApplicationServices.CreateScope();
            using BankingDbContext dbContext = scope.ServiceProvider.GetRequiredService<BankingDbContext>();

            DbSeeder.Seed(dbContext);
        }
    }
}
