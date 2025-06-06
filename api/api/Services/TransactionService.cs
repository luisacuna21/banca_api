using api.IServices;
using api.Models;
using api.Models.Complements;
using api.Models.DTOs.TransactionDTOs;
using api.Utilities;
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

        // Add this private method to the TransactionService class
        private async Task<bool> EnsureSufficientFundsIfDebitAsync(string accountNumber, decimal amount)
        {
            var currentBalance = await _accountService.GetCurrentBalanceByAccountNumberAsync(accountNumber);
            return currentBalance > amount;
        }

        private void EnsureAmountBiggerThanZero(decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Monto inválido. No puede registrarse una transacción con monto negativo y no puede ser cero.");
            }
        }

        /// <summary>
        /// Checks if a <c>TransactionType</c> exists and returns it. If not, throw an exception.
        /// </summary>
        /// <param name="transactionTypeName"></param>
        /// <returns>The existing <c>TransactionType</c></returns>
        /// <exception cref="ArgumentException"></exception>
        private async Task<TransactionType> EnsureTransactionTypeExists(string transactionTypeName)
        {
            var transactionType = await _context.TransactionTypes.FindAsync(transactionTypeName);

            // Verify if requested transaction type exists
            if (transactionType == null)
            {
                throw new ArgumentException("Tipo de transacción inválido.");
            }

            return transactionType;
        }

        public async Task<TransactionDTO> CreateSimpleTransactionAsync(SimpleTransactionCreateRequest simpleTransaction)
        {
            EnsureAmountBiggerThanZero(simpleTransaction.Amount);

            var transactionType = await EnsureTransactionTypeExists(simpleTransaction.TransactionTypeName);

            // Verify if requested transaction type is simple, if not, throw an error
            if (!transactionType.Simple)
            {
                throw new ArgumentException($"Tipo de transacción ({simpleTransaction.TransactionTypeName}) no admitido.");
            }

            // Check if the account exist
            var account = await _accountService.GetAccountByAccountNumberAsync(simpleTransaction.AccountNumber);
            if (account == null)
            {
                throw new InvalidOperationException("La cuenta no existe.");
            }

            // Verify if the balance effect of the transaction type is Debit and if the account has sufficient funds
            if (transactionType.BalanceEffect == BalanceEffect.Debit
                && !await EnsureSufficientFundsIfDebitAsync(simpleTransaction.AccountNumber, simpleTransaction.Amount))
            {
                throw new InvalidOperationException("Fondos insuficientes.");
            }

            var transaction = _mapper.Map<Transaction>(simpleTransaction);
            transaction.AccountId = account.Id;

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            return _mapper.Map<TransactionDTO>(transaction);
        }

        public async Task<TransactionDTO> CreateInterestTransactionAsync(InterestTransactionCreateRequest interestTransaction)
        {
            var account = await _accountService.GetAccountByAccountNumberAsync(interestTransaction.AccountNumber);
            if (account == null)
            {
                throw new InvalidOperationException("La cuenta no existe.");
            }

            var currentBalance = await _accountService.GetCurrentBalanceByAccountNumberAsync(interestTransaction.AccountNumber);
            if (currentBalance == 0)
            {
                throw new InvalidOperationException("No se puede aplicar interés a una cuenta con saldo cero.");
            }

            // Convert annual rate to monthly
            var monthlyInterestRate = (account.AnnualInterestRate / 100m) / 12m;

            var interestAmount = currentBalance * monthlyInterestRate;

            var interestTransactionRecord = new InterestTransaction
            {
                AccountId = account.Id,
                Amount = interestAmount,
                TimeStamp = DateTime.Now,
                Description = interestTransaction.Description,
                TransactionTypeName = TransactionTypeNames.Interest,
                InterestRate = account.AnnualInterestRate
            };

            _context.Transactions.Add(interestTransactionRecord);
            await _context.SaveChangesAsync();

            return _mapper.Map<TransactionDTO>(interestTransactionRecord);
        }

        public async Task<TransactionDTO> CreateTransferTransactionAsync(TransferTransactionCreateRequest transferTransaction)
        {
            EnsureAmountBiggerThanZero(transferTransaction.Amount);

            // TransactionType BalanceEffect is always Debit for transactions, so It is need to check if the account
            // has sufficient funds.
            var sufficientFunds = await EnsureSufficientFundsIfDebitAsync(transferTransaction.AccountNumber, transferTransaction.Amount);

            if (!sufficientFunds)
            {
                throw new InvalidOperationException("Fondos insuficientes.");
            }

            // Check if the destination account is not set.
            if (String.IsNullOrEmpty(transferTransaction.DestinationAccountNumber))
            {
                throw new InvalidOperationException("Debe especificar una cuenta de destino.");
            }

            // Check if the source and destination account exist
            var sourceAccount = await _accountService.GetAccountByAccountNumberAsync(transferTransaction.AccountNumber);
            var destinationAccount = await _accountService.GetAccountByAccountNumberAsync(transferTransaction.DestinationAccountNumber);

            if (sourceAccount == null)
            {
                throw new InvalidOperationException("La cuenta de origen no existe.");
            }

            if (destinationAccount == null)
            {
                throw new InvalidOperationException("La cuenta de destino no existe.");
            }

            // Save transaction for source account (transfer_out)
            var transferRecordForSourceAccount = _mapper.Map<TransferOutTransaction>(transferTransaction);
            // Set Transaction Type Name here (transfer_out) for source account
            transferRecordForSourceAccount.TransactionTypeName = TransactionTypeNames.TransferOut;
            // Set the Account Id to the Transaction record
            transferRecordForSourceAccount.AccountId = sourceAccount.Id;
            // Set the DestinationAccount Id to the Transaction record
            transferRecordForSourceAccount.DestinationAccountId = destinationAccount.Id;
            // Set the TransactionTypeName to "transfer_out"
            transferRecordForSourceAccount.TransactionTypeName = TransactionTypeNames.TransferOut;

            _context.Transactions.Add(transferRecordForSourceAccount);
            await _context.SaveChangesAsync();


            // Save transaction for destination account (transfer_in)
            var transferRecordForDestinationAccount
                = new TransferInTransaction
            {
                // The DestinationAccountId of the request turns into the AccountId of the destination Account
                AccountId = destinationAccount.Id,
                // The AccountId of the request turns into the SourceAccountId of the destination account
                SourceAccountId = sourceAccount.Id,
                // The transaction type is TransferIn (transfer_in) for the destination account
                TransactionTypeName = TransactionTypeNames.TransferIn,
                Amount = transferTransaction.Amount,
                TimeStamp = DateTime.Now,
                Description = transferTransaction.Description
            };

            _context.Transactions.Add(transferRecordForDestinationAccount);
            await _context.SaveChangesAsync();

            // Return the record for the source account
            return _mapper.Map<TransactionDTO>(transferRecordForSourceAccount);
        }

        // TODO: REMOVE THIS
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

        public async Task<IEnumerable<TransactionDTO>?> GetTransactionsByAccountNumber(string accountNumber)
        {
            // Validate account number
            if (string.IsNullOrWhiteSpace(accountNumber))
                throw new ArgumentException("Número de cuenta inválido.");

            var transactions = await _context.Transactions
                .Include(transaction => transaction.TransactionType)
                .Include(transaction => transaction.Account)
                .Where(transaction => transaction.Account.AccountNumber == accountNumber)
                .ToListAsync();

            // If no transactions found, return null
            if (transactions == null || !transactions.Any())
                return null;

            //return transactions;
            return _mapper.Map<IEnumerable<TransactionDTO>>(transactions);
        }
    }
}
