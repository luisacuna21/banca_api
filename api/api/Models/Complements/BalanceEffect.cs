namespace api.Models.Complements
{
    /// <summary>
    /// Represents the effect of a transaction on the account balance.
    /// Can be either a Credit (increase) with a value of 1, or a Debit (decrease) with a value of -1.
    /// </summary>
    public enum BalanceEffect
    {
        // Increses the balance
        Credit = 1,
        // Decreases the balance
        Debit = -1
    }
}
