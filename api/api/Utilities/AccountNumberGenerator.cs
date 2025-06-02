using api.Models;

namespace api.Utilities
{
    public static class AccountNumberGenerator
    {
        private static readonly Random _random = new();

        public static string GenerateUniqueAccountNumber(BankingDbContext dbContext, int length = 9)
        {
            string accountNumber;
            do
            {
                accountNumber = GenerateRandomNumberString(length);
            }
            while (dbContext.Accounts.Any(a => a.AccountNumber == accountNumber));
            return accountNumber;
        }

        private static string GenerateRandomNumberString(int length)
        {
            // Ensure the first digit is not zero
            var number = _random.Next(1, 10).ToString();
            for (int i = 1; i < length; i++)
            {
                number += _random.Next(0, 10).ToString();
            }
            return number;
        }
    }
}
