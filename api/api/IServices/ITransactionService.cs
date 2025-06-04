using api.Models;
using api.Models.DTOs.TransactionDTOs;

namespace api.IServices
{
    public interface ITransactionService
    {
        Task<IEnumerable<TransactionDTO>?> GetAllByAccountIdAsync(int accountId);
        Task<TransactionDTO?> GetByIdAsync(int id);
        Task<TransactionDTO> CreateAsync(TransactionCreateRequest transactionDTO);
    }
}
