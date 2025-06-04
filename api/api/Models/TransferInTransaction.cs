namespace api.Models
{
    public class TransferInTransaction : Transaction
    {
        public int SourceAccountId { get; set; }
        public Account SourceAccount { get; set; } = default!;
    }
}
