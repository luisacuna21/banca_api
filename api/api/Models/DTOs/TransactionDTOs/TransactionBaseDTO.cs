using System.ComponentModel.DataAnnotations;

namespace api.Models.DTOs.TransactionDTOs
{
    public class TransactionBaseDTO
    {
        public string AccountNumber { get; set; }

        [Required(ErrorMessage = "El tipo de transacción es obligatorio.")]
        public string TransactionTypeName { get; set; }
        
        public decimal Amount { get; set; }
        
        public string? Description { get; set; }
    }
}
