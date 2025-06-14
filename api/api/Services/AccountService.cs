﻿using api.IServices;
using api.Models;
using api.Models.DTOs.AccountDTOs;
using api.Models.DTOs.CustomerDTOs;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace api.Services
{
    public class AccountService : IAccountService
    {
        private readonly BankingDbContext _context;
        private readonly IMapper _mapper;

        public AccountService(BankingDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AccountDTO> CreateAsync(AccountCreateRequest createRequest)
        {
            var account = _mapper.Map<Account>(createRequest);
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();

            var accountDTO = _mapper.Map<AccountDTO>(account);
            return accountDTO;
        }

        // TODO: Check if this implementation is better than GetAllAsync
        public async Task<IEnumerable<AccountDTO>?> GetAccountsByCustomerIdAsync(int customerId)
        {
            var accounts = await _context.Accounts
                .Where(account => account.CustomerId == customerId)
                .Include(account => account.Transactions)
                    .ThenInclude(transaction => transaction.TransactionType)
                .ToListAsync();

            var accountDTOs = accounts.Select(account =>
            {
                var currentBalance = account.InitialBalance +
                    (account.Transactions != null
                        ? account.Transactions.Sum(t =>
                            t.TransactionType.BalanceEffect == Models.Complements.BalanceEffect.Credit ? t.Amount : -t.Amount)
                        : 0);

                var dto = _mapper.Map<AccountDTO>(account);
                dto.CurrentBalance = currentBalance;
                return dto;
            }).ToList();

            return accountDTOs;
        }

        public async Task<IEnumerable<AccountDTO>?> GetAllAsync()
        {
            var accounts = await _context.Accounts
                .Include(account => account.Transactions)
                    .ThenInclude(transaction => transaction.TransactionType)
                .ToListAsync();

            var accountDTOs = _mapper.Map<IEnumerable<AccountDTO>>(accounts);
            return accountDTOs;
            //var accounts = await _context.Accounts
            //    .Include(account => account.Transactions)
            //        .ThenInclude(transaction => transaction.TransactionType)
            //    .ToListAsync();

            //var accountDTOs = accounts.Select(account =>
            //{
            //    var currentBalance = account.InitialBalance +
            //        (account.Transactions != null
            //            ? account.Transactions.Sum(t =>
            //                t.TransactionType.BalanceEffect == Models.Complements.BalanceEffect.Credit ? t.Amount : -t.Amount)
            //            : 0);

            //    var dto = _mapper.Map<AccountDTO>(account);
            //    dto.CurrentBalance = currentBalance;
            //    return dto;
            //}).ToList();

            //return accountDTOs;
        }


        public async Task<AccountDTO?> GetByIdAsync(int id)
        {
            var account = await _context.Accounts
                .Include(account => account.Transactions)
                    .ThenInclude(transaction => transaction.TransactionType)
                .FirstOrDefaultAsync(account => account.Id == id);

            var accountDTO = _mapper.Map<AccountDTO>(account);
            return accountDTO;
        }

        public async Task<AccountDTO?> GetAccountByAccountNumberAsync(string accountNumber)
        {
            var account = await _context.Accounts
                .Include(account => account.Transactions)
                    .ThenInclude(transaction => transaction.TransactionType)
                .FirstOrDefaultAsync(account => account.AccountNumber == accountNumber);

            return _mapper.Map<AccountDTO>(account);
        }

        // TODO: REMOVE THIS
        public async Task<decimal> GetCurrentBalanceByAccountIdAsync(int accountId)
        {
            var account = await _context.Accounts
                .Where(account => account.Id == accountId)
                .Include(account => account.Transactions)
                    .ThenInclude(transaction => transaction.TransactionType)
                .FirstOrDefaultAsync();

            var accountDTO = _mapper.Map<AccountDTO>(account);
            var currentBalance = accountDTO.CurrentBalance;
            return currentBalance;
        }
        public async Task<decimal> GetCurrentBalanceByAccountNumberAsync(string accountNumber)
        {
            var account = await _context.Accounts
                .Where(account => account.AccountNumber == accountNumber)
                .Include(account => account.Transactions)
                    .ThenInclude(transaction => transaction.TransactionType)
                .FirstOrDefaultAsync();

            var accountDTO = _mapper.Map<AccountDTO>(account);
            var currentBalance = accountDTO.CurrentBalance;
            return currentBalance;
        }

        public async Task<bool> CheckIfExistsByAccountNumber(string accountNumber)
        {
            var account = await _context.Accounts.FirstOrDefaultAsync(account => account.AccountNumber == accountNumber);
            return account != null;
        }
    }
}
