using api.IServices;
using api.Models;
using api.Models.Complements;
using api.Models.DTOs.TransactionDTOs;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace api.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly BankingDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAccountService _accountService;

        public TransactionService(BankingDbContext context, IMapper mapper, IAccountService accountService)
        {
            _context = context;
            _mapper = mapper;
            _accountService = accountService;
        }

        // TODO: Create different methods for creating transactions based on special types
        // such as transfers, interest, etc. to avoid the switch-case complexity.
        // And also provide a more complete, clear and requested structure in the API Endpoint
        public async Task<TransactionDTO> CreateAsync(TransactionCreateRequest createRequest)
        {
            // Validate amount > 0
            if (createRequest.Amount <= 0)
            {
                throw new ArgumentException("Monto inválido. No puede registrarse una transacción con monto negativo.");
            }

            var transactionType = await _context.TransactionTypes
                .FindAsync(createRequest.TransactionTypeName);

            // Verify if requested transaction type exists
            if (transactionType == null)
            {
                throw new ArgumentException("Tipo de transacción inválido.");
            }

            var currentBalance = await _accountService.GetCurrentBalanceByAccountIdAsync(createRequest.AccountId);

            // If transaction type is Debit then check if there are sufficient funds
            if (transactionType.BalanceEffect == BalanceEffect.Debit
                && createRequest.Amount > currentBalance)
            {
                throw new InvalidOperationException("Fondos insuficientes.");
            }

            switch (transactionType.Name)
            {
                case TransactionTypeNames.Transfer:

                    // Check if the destination account is not set.
                    if (createRequest.DestinationAccountId == 0)
                    {
                        throw new InvalidOperationException("Debe especificar una cuenta de destino.");
                    }

                    var transferOut = _mapper.Map<TransferInTransaction>(createRequest);
                    // Set the TransactionTypeName to "transfer_out"
                    transferOut.TransactionTypeName = TransactionTypeNames.TransferOut;

                    // Save transaction for source account (transfer_out)
                    _context.Transactions.Add(transferOut);
                    await _context.SaveChangesAsync();

                    // Save transaction for destination account (transfer_in)
                    var transferIn = new TransferOutTransaction
                    {
                        // The DestinationAccountId of the request turns into the AccountId of the destination Account
                        AccountId = createRequest.DestinationAccountId,
                        // The AccountId of the request turns into the SourceAccountId of the destination account
                        SourceAccountId = createRequest.AccountId,
                        // The transaction type changes to TransferIn (transfer_in) for the destination account
                        TransactionTypeName = TransactionTypeNames.TransferIn,
                        Amount = createRequest.Amount,
                        TimeStamp = DateTime.Now,
                        Description = createRequest.Description
                    };
                    
                    _context.Transactions.Add(transferIn);
                    await _context.SaveChangesAsync();

                    return _mapper.Map<TransactionDTO>(transferIn);

                // TODO: Add handling for interest transactions
                default:
                    var transaction = _mapper.Map<Transaction>(createRequest);

                    _context.Add(transaction);
                    await _context.SaveChangesAsync();

                    return _mapper.Map<TransactionDTO>(transaction);
            }
        }

        public async Task<IEnumerable<TransactionDTO>?> GetAllByAccountIdAsync(int accountId)
        {
            var transactions = await _context.Transactions
                .Include(transaction => transaction.TransactionType)
                .Where(transaction => transaction.AccountId == accountId)
                .ToListAsync();

            //return transactions;
            return _mapper.Map<IEnumerable<TransactionDTO>>(transactions);
        }

        public async Task<TransactionDTO?> GetByIdAsync(int id)
        {
            var transaction = await _context.Transactions
                .Include(transaction => transaction.TransactionType)
                .FirstOrDefaultAsync();

            //return transaction;
            return _mapper.Map<TransactionDTO>(transaction);
        }
    }
}
