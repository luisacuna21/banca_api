namespace api.Models.DTOs.TransactionDTOs
{
    public class TransactionBaseDTO
    {
        public string AccountNumber { get; set; }
        
        public decimal Amount { get; set; }
        
        public string? Description { get; set; }
    }
}
