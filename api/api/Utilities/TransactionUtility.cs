using api.Models;

namespace api.Utilities
{
    public static class TransactionUtility
    {
        public static bool AmountPositive(decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Monto inválido. No puede registrarse una transacción con monto negativo y no puede ser cero.");
            }

            return amount > 0;
        }
    }
}
