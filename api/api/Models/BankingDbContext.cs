﻿using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace api.Models
{
    public class BankingDbContext : DbContext
    {
        public BankingDbContext(DbContextOptions<BankingDbContext> options) : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<TransactionType> TransactionTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Account>()
                .HasIndex(a => a.AccountNumber)
                .IsUnique();

            modelBuilder.Entity<Account>()
                .HasOne(a => a.Customer)
                .WithMany(c => c.Accounts)
                .HasForeignKey(a => a.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TransactionType>()
                .HasKey(tt => tt.Name);

            modelBuilder.Entity<Transaction>()
            .HasDiscriminator<string>("TransferDiscriminator")
            .HasValue<Transaction>("Base")
            .HasValue<TransferOutTransaction>("Transfer_Record_For_Source_Account")
            .HasValue<TransferInTransaction>("Transfer_Record_For_Destination_Account")
            .HasValue<InterestTransaction>("Interest");

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.TransactionType)
                .WithMany(tt => tt.Transactions)
                .HasForeignKey(t => t.TransactionTypeName)
                .HasPrincipalKey(tt => tt.Name);
        }
    }
}
