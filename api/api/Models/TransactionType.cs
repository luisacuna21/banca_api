using api.Models.Complements;

namespace api.Models
{
    public class TransactionType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public BalanceEffect BalanceEffect { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        public List<Transaction> Transactions { get; set; }
    }
}
