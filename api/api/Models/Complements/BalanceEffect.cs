namespace api.Models.Complements
{
    /// <summary>
    /// Represents the effect of a transaction on the account balance.
    /// </summary>
    public enum BalanceEffect
    {
        // Increses the balance
        Credit = 1,
        // Decreases the balance
        Debit = -1
    }
}
