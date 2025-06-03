using System.ComponentModel.DataAnnotations;

namespace api.Models
{
    public class Transaction
    {
        public int Id { get; set; }

        public int AccountId { get; set; }
        public Account Account { get; set; } = default!;


        [Required(ErrorMessage = "El tipo de transacción es obligatorio.")]
        public string TransactionTypeName { get; set; }
        public TransactionType TransactionType { get; set; }

        
        [Range(0.01, double.MaxValue, ErrorMessage = "La cantidad de la transacción debe ser mayor a cero.")]
        public decimal Amount { get; set; }
        
        public DateTime TimeStamp { get; set; } = DateTime.Now;
        
        public string? Description { get; set; }
    }
}
