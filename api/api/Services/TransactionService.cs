using api.IServices;
using api.Models;
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

        public async Task<TransactionDTO> CreateAsync(TransactionCreateRequest createRequest)
        {
            // Validate amount > 0
            if (createRequest.Amount <= 0)
            {
                throw new ArgumentException("Monto inválido. No se puede registrarse una transacción con monto negativo.");
            }

            var transactionType = await _context.TransactionTypes.FindAsync(createRequest.TransactionTypeId);
            var transferTransactionType = await _context.TransactionTypes.FirstOrDefaultAsync(transactionType => transactionType.Name.ToLower() == "transferencia");

            // Verify if requested transaction type exists
            if (transactionType == null)
            {
                throw new ArgumentException("Tipo de transacción inválido.");
            }

            // Verify if "transferencia" transaction type exists
            if (transferTransactionType == null)
            {
                throw new InvalidOperationException("Tipo de transacción inválido.");
            }

            // Verify if requested transaction type is transfer and destination account is specified
            if (createRequest.TransactionTypeId == transferTransactionType.Id
                && createRequest.DestinationAccountId == 0)
            {
                throw new InvalidOperationException("Debe especificar una cuenta de destino.");
            }

            var transaction = _mapper.Map<Transaction>(createRequest);

            // Get account current balance
            var currentBalance = await _accountService.GetCurrentBalanceByAccountIdAsync(transaction.SourceAccountId);

            // If transaction type is Debit, check if there are sufficient funds
            if (transaction.TransactionType.BalanceEffect == Models.Complements.BalanceEffect.Debit &&
                transaction.Amount > currentBalance)
            {
                throw new InvalidOperationException("Fondos insuficientes.");
            }

            // Save transaction
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            return _mapper.Map<TransactionDTO>(transaction);
        }

        public async Task<IEnumerable<TransactionDTO>?> GetAllByAccountIdAsync(int accountId)
        {
            var transactions = await _context.Transactions
                .Include(transaction => transaction.TransactionType)
                .Where(transaction => transaction.SourceAccountId == accountId)
                .ToListAsync();

            return _mapper.Map<IEnumerable<TransactionDTO>>(transactions);
        }

        public async Task<TransactionDTO?> GetByIdAsync(int id)
        {
            var transaction = await _context.Transactions
                .Include(transaction => transaction.TransactionType)
                .FirstOrDefaultAsync();

            return _mapper.Map<TransactionDTO>(transaction);
        }
    }
}
