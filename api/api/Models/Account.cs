namespace api.Models
{
    public class Account
    {
        public int Id { get; set; }
        public string AccountNumber { get; set; }
        public decimal InitialBalance { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public DateTime CreatedAt { get; set; } = new DateTime();

        public List<Transaction> Transactions { get; set; }
    }
}
