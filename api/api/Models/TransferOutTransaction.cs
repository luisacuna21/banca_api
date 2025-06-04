namespace api.Models
{
    public class TransferOutTransaction : Transaction
    {
        public int DestinationAccountId { get; set; }
        public Account DestinationAccount { get; set; } = default!;
    }
}
