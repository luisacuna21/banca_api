namespace api.Models.DTOs.CustomerDTOs
{
    public abstract class CustomerBaseDTO
    {
        public string? Name { get; set; }
        public DateTime? Birthdate { get; set; }
        public decimal? Incomes { get; set; }
    }
}
