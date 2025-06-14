﻿using api.Models;
using api.Models.Complements;
using api.Models.DTOs.AccountDTOs;
using api.Models.DTOs.CustomerDTOs;
using api.Models.DTOs.TransactionDTOs;
using AutoMapper;

namespace api.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Mapping configurations for Customer and its DTOs
            CreateMap<Customer, CustomerDTO>();
            CreateMap<CustomerCreateRequest, Customer>();
            CreateMap<CustomerUpdateRequest, Customer>()
                .ForMember(dest => dest.Name, opt => opt.Condition(src => src.Name != null))
                .ForMember(dest => dest.Birthdate, opt => opt.Condition(src => src.Birthdate.HasValue))
                .ForMember(dest => dest.Incomes, opt => opt.Condition(src => src.Incomes.HasValue));

            // Mapping configurations for Account and its DTOs
            //CreateMap<Account, AccountDTO>().ReverseMap();
            CreateMap<Account, AccountDTO>()
                .ForMember(
                    dest => dest.CurrentBalance,
                    opt => opt.MapFrom(src =>
                        src.InitialBalance +
                        (src.Transactions != null
                            ? src.Transactions.Sum(t => (t.TransactionType.BalanceEffect == BalanceEffect.Credit
                                ? t.Amount : -t.Amount))
                            : 0)
                    )
                );
            CreateMap<AccountCreateRequest, Account>();

            // Mapping configuration for Transaction and its DTOs
            CreateMap<Transaction, TransactionDTO>()
                .ForMember(
                    dest => dest.BalanceEffect,
                    opt => opt.MapFrom(src => src.TransactionType.BalanceEffect)
                );
            // Configuration for mapping Simple transactions create requests to transaction (such as deposits, withdrawals, and so on)
            CreateMap<SimpleTransactionCreateRequest, Transaction>();
            CreateMap<TransferTransactionCreateRequest, TransferOutTransaction>();
        }
    }
}
