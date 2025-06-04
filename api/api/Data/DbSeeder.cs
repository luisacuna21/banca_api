using api.Models;
using api.Models.Complements;

namespace api.Data
{
    public static class DbSeeder
    {
        public static void Seed(BankingDbContext dbContext)
        {
            // TransactionTypes
            if (!dbContext.TransactionTypes.Any())
            {
                dbContext.TransactionTypes.AddRange(
                    new TransactionType
                    {
                        Name = "deposit",
                        Description = "Depósito a la cuenta",
                        BalanceEffect = BalanceEffect.Credit
                    },
                    new TransactionType
                    {
                        Name = "withdrawal",
                        Description = "Retiro desde la cuenta",
                        BalanceEffect = BalanceEffect.Debit
                    },
                    new TransactionType
                    {
                        Name = "transfer",
                        Description = "Virtual, utilizado para solicitudes a la API. Transferencia entre cuentas",
                        BalanceEffect = BalanceEffect.Debit
                    },
                    new TransactionType
                    {
                        Name = "transfer_in",
                        Description = "Transferencia recibida desde otra cuenta",
                        BalanceEffect = BalanceEffect.Credit
                    },
                    new TransactionType
                    {
                        Name = "transfer_out",
                        Description = "Transferencia enviada a otra cuenta",
                        BalanceEffect = BalanceEffect.Debit
                    },
                    new TransactionType
                    {
                        Name = "interest",
                        Description = "Intereses generados en la cuenta",
                        BalanceEffect = BalanceEffect.Credit
                    }
                );
                dbContext.SaveChanges();
            }

            // Customers
            if (!dbContext.Customers.Any())
            {
                dbContext.Customers.AddRange(
                    new Customer
                    {
                        Name="Luis Gabriel Acuña Porras",
                        Birthdate = new DateTime(2000, 9, 6),
                        Incomes = 1000.00m
                    },
                    new Customer
                    {
                        Name = "María Joaquina Acuña Porras",
                        Birthdate = new DateTime(1994, 5, 22),
                        Incomes = 1500.00m
                    }
                );
                dbContext.SaveChanges();
            }

            // Accounts
            if (!dbContext.Accounts.Any())
            {
                dbContext.Accounts.AddRange(
                    new Account
                    {
                        AccountNumber = "119294840",
                        CustomerId = 1,
                        InitialBalance = 10000.00m,
                        AnnualInterestRate = 5.0m // 5% interest rate
                    },
                    new Account
                    {
                        AccountNumber = "098765432",
                        CustomerId = 2,
                        InitialBalance = 20000.00m,
                        AnnualInterestRate = 3.5m // 3.5% interest rate
                    }
                );
                dbContext.SaveChanges();
            }
        }
    }
}
