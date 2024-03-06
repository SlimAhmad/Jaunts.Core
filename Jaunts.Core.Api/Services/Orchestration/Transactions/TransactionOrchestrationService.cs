// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Jaunts.Core.Api.Brokers.Loggings;
using Jaunts.Core.Api.Models.Services.Foundations.Transactions;
using Jaunts.Core.Api.Models.Services.Foundations.WalletBalances;
using Jaunts.Core.Api.Models.Services.Foundations.Wallets;
using Jaunts.Core.Api.Services.Processings.Transactions;
using Jaunts.Core.Api.Services.Processings.WalletBalances;
using Jaunts.Core.Api.Services.Processings.Wallets;

namespace Jaunts.Core.Api.Services.Orchestration.Transactions
{
    public partial class TransactionOrchestrationService : ITransactionOrchestrationService
    {
        private readonly ITransactionProcessingService transactionProcessingService;
        private readonly IWalletProcessingService walletProcessingService;
        private readonly IWalletBalanceProcessingService walletBalanceProcessingService;
        private readonly ILoggingBroker loggingBroker;
        

        public TransactionOrchestrationService(
            ITransactionProcessingService TransactionProcessingService,
            IWalletProcessingService WalletProcessingService,
            ILoggingBroker loggingBroker
           )
        {
            this.transactionProcessingService =  TransactionProcessingService ;
            this.walletProcessingService = WalletProcessingService;
            this.loggingBroker = loggingBroker;

        }

        public ValueTask<bool> RemoveTransactionByIdAsync(Guid id) =>
        TryCatch(async () => await this.transactionProcessingService.RemoveTransactionAsync(id));
        public IQueryable<Transaction> RetrieveAllTransactionsAsync() =>
        TryCatch(() => this.transactionProcessingService.RetrieveAllTransactions());
        public ValueTask<Transaction> RetrieveTransactionAsync(Guid id) =>
        TryCatch(async () => await this.transactionProcessingService.RetrieveTransactionByIdAsync(id));
        public ValueTask<Transaction> UpsertTransactionAsync(Transaction Transaction) =>
        TryCatch(async () => await this.transactionProcessingService.UpsertTransactionAsync(Transaction));

    }
}
