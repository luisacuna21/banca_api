using System.ComponentModel.DataAnnotations;

namespace api.Models.DTOs.TransactionDTOs
{
    public class TransferTransactionCreateRequest : TransactionBaseDTO
    {
        [Required(ErrorMessage = "El número de cuenta de destino es obligatorio.")]
        public string DestinationAccountNumber { get; set; }

        public decimal Amount { get; set; }
    }
}
