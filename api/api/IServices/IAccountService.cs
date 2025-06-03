using api.Models.DTOs.AccountDTOs;

namespace api.IServices
{
    public interface IAccountService
    {
        Task<IEnumerable<AccountDTO>> GetAccountsByCustomerIdAsync(int customerId);
        Task<IEnumerable<AccountDTO>> GetAllAsync();
        Task<AccountDTO?> GetByIdAsync(int id);
        Task<AccountDTO?> GetAccountByAccountNumber(string accountNumber);
        Task<AccountDTO> CreateAsync(AccountCreateRequest createRequest);
        Task<decimal> GetCurrentBalanceByAccountIdAsync(int accountId);
    }
}
