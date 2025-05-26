using System.ComponentModel.DataAnnotations;

namespace api.Models
{
    public class Account
    {
        public int Id { get; set; }
        
        [StringLength(9, ErrorMessage = "El número de cuenta no puede exceder los 20 caracteres")]
        public string AccountNumber { get; set; }
        
        public decimal InitialBalance { get; set; }

        /// <summary>
        /// The annual interest rate for the account, expressed as a percentage.
        /// For example: 5.0 for 5% interest rate.
        /// </summary>
        public decimal AnnualInterestRate { get; set; }

        public int CustomerId { get; set; }
        
        public Customer Customer { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public List<Transaction> Transactions { get; set; }
    }
}
