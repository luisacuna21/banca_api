using api.Models.Complements;

namespace api.Models.DTOs.TransactionDTOs
{
    public class TransactionDTO : TransactionBaseDTO
    {
        public int Id { get; set; }
        public DateTime TimeStamp { get; set; }
        public BalanceEffect BalanceEffect { get; set; }
    }
}
