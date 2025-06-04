using api.Models;
using api.Models.DTOs.CustomerDTOs;

namespace api.IServices
{
    public interface ICustomerService
    {
        Task<IEnumerable<CustomerDTO>?> GetAllAsync();
        Task<CustomerDTO?> GetByIdAsync(int id);
        Task<CustomerDTO> CreateAsync(CustomerCreateRequest createRequest);
        Task<bool> UpdateAsync(int id, CustomerUpdateRequest updateRequest);
    }
}
