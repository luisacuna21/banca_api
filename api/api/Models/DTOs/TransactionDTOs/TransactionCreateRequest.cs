namespace api.Models.DTOs.TransactionDTOs
{
    public class TransactionCreateRequest : TransactionBaseDTO
    {
        public int DestinationAccountId { get; set; }
    }
}
