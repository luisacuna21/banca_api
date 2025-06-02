namespace api.Models.DTOs.CustomerDTOs
{
    public class CustomerDTO : CustomerBaseDTO
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
