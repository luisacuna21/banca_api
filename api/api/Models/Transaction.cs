using System.ComponentModel.DataAnnotations;

namespace api.Models
{
    public class Transaction
    {
        public int Id { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "La cantidad de la transacción debe ser mayor a cero.")]
        public decimal Amount { get; set; }
        public DateTime TimeStamp { get; set; } = DateTime.Now;
        
        public int TransactionTypeId { get; set; }
        public TransactionType TransactionType { get; set; }

        // For transfers
        public int SourceAccountId { get; set; }
        public Account SourceAccount { get; set; } = default!;

        public int? DestinationAccountId { get; set; }
        public Account? DestinationAccount { get; set; }
        public string? Description { get; set; }
    }
}
