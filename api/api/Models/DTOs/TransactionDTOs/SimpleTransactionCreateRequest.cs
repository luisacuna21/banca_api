using System.ComponentModel.DataAnnotations;

namespace api.Models.DTOs.TransactionDTOs
{
    public class SimpleTransactionCreateRequest : TransactionBaseDTO
    {
        [Required(ErrorMessage = "El tipo de transacción es obligatorio.")]
        public string TransactionTypeName { get; set; }
    }
}
