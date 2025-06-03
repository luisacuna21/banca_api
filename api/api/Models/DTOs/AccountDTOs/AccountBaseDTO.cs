namespace api.Models.DTOs.AccountDTOs
{
    public class AccountBaseDTO
    {
        public decimal InitialBalance { get; set; }
        public decimal AnnualInterestRate { get; set; }
        public int CustomerId { get; set; }
    }
}
