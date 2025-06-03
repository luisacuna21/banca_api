namespace api.Models
{
    public class TransferTransaction : Transaction
    {
        public int DestinationAccountId { get; set; }
        public Account DestinationAccount { get; set; } = default!;
    }
}
