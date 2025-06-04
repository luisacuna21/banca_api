namespace api.Models
{
    public class TransferOutTransaction : Transaction
    {
        public int SourceAccountId { get; set; }
        public Account SourceAccount { get; set; } = default!;
    }
}
