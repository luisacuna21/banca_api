namespace api.Models.DTOs.TransactionDTOs
{
    public class TransactionBaseDTO
    {
        public decimal Amount { get; set; }
        public int TransactionTypeId { get; set; }
        public int SourceAccountId { get; set; }
        public int? DestinationAccountId { get; set; }
        public string? Description { get; set; }
    }
}
