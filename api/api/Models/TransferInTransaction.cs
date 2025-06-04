namespace api.Models
{
    public class TransferInTransaction : Transaction
    {
        public int DestinationAccountId { get; set; }
        public Account DestinationAccount { get; set; } = default!;
    }
}
