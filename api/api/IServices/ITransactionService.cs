using api.Models;
using api.Models.DTOs.TransactionDTOs;

namespace api.IServices
{
    public interface ITransactionService
    {
        /// <summary>
        /// Creates a simple transaction, which does not require additional information beyond the 
        /// <c>Transaction</c> base class fields. That simple transactions do not require complex validations.
        /// </summary>
        /// <param name="deposit"></param>
        /// <returns></returns>
        Task<TransactionDTO> CreateSimpleTransactionAsync(SimpleTransactionCreateRequest simpleTransaction);

        /// <summary>
        /// Creates a transfer transaction, which requires additional information such as the destination account number.
        /// </summary>
        /// <param name="transfer"></param>
        /// <returns></returns>
        Task<TransactionDTO> CreateTransferTransactionAsync(TransferTransactionCreateRequest transferTransaction);

        /// <summary>
        /// Creates an interest transaction, which requires additional information such as the interest rate.
        /// </summary>
        /// <param name="interestTransaction"></param>
        /// <returns></returns>
        Task<TransactionDTO> CreateInterestTransactionAsync(InterestTransactionCreateRequest interestTransaction);

        /// <summary>
        /// Retrieves all transactions associated with a specific account number.
        /// </summary>
        /// <param name="accountNumber"></param>
        /// <returns></returns>
        Task<IEnumerable<TransactionDTO>?> GetTransactionsByAccountNumber(string accountNumber);
        
        Task<TransactionDTO?> GetByIdAsync(int id);

        // TODO: REMOVE THIS ALSO
        //Task<TransactionDTO> CreateAsync(TransactionCreateRequest transactionDTO);

    }
}
