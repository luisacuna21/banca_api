namespace api.Models.DTOs.AccountDTOs
{
    public class AccountDTO : AccountBaseDTO
    {
        public int Id { get; set; }
        public string AccountNumber { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public decimal CurrentBalance { get; set; }
    }
}
