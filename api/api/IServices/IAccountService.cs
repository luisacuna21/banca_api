using api.Models.DTOs.AccountDTOs;

namespace api.IServices
{
    public interface IAccountService
    {
        Task<IEnumerable<AccountDTO>?> GetAccountsByCustomerIdAsync(int customerId);
        Task<IEnumerable<AccountDTO>?> GetAllAsync();
        Task<AccountDTO?> GetByIdAsync(int id);
        Task<AccountDTO?> GetAccountByAccountNumberAsync(string accountNumber);
        Task<AccountDTO> CreateAsync(AccountCreateRequest createRequest);

        // TODO: REMOVE THIS
        Task<decimal> GetCurrentBalanceByAccountIdAsync(int accountId);

        Task<decimal> GetCurrentBalanceByAccountNumberAsync(string accountNumber);

        Task<bool> CheckIfExistsByAccountNumber(string accountNumber);
    }
}
