using Jaunts.Core.Api.Brokers.Loggings;
using Jaunts.Core.Api.Models.Services.Foundations.Transactions;
using Jaunts.Core.Api.Services.Foundations.Transactions;
using System.Linq.Expressions;

namespace Jaunts.Core.Api.Services.Processings.Transactions
{
    public partial class TransactionProcessingService : ITransactionProcessingService
    {
      
        private readonly ITransactionService  transactionService;
        private readonly ILoggingBroker loggingBroker;

        public TransactionProcessingService(
            ITransactionService  TransactionService,
            ILoggingBroker loggingBroker

            )
        {
            this.transactionService = TransactionService;
            this.loggingBroker = loggingBroker;

        }

        public IQueryable<Transaction> RetrieveAllTransactions() =>
        TryCatch(() => this.transactionService.RetrieveAllTransactions());
        public ValueTask<bool> RemoveTransactionAsync(
            Guid id) =>
        TryCatch(async () =>
        {
            ValidateTransactionId(id);
            var transaction = await transactionService.RemoveTransactionByIdAsync(id);
            ValidateTransaction(transaction);
            return true;
        });
        public ValueTask<Transaction> RetrieveTransactionByIdAsync(Guid id) =>
        TryCatch(async () =>
        {
            ValidateTransactionId(id);
            var wallet = await transactionService.RetrieveTransactionByIdAsync(id);
            ValidateTransaction(wallet);
            return wallet;
        });

        public ValueTask<Transaction> UpsertTransactionAsync(
            Transaction  transaction) =>
        TryCatch(async () =>
        {
            ValidateTransaction(transaction);
            Transaction maybeTransaction = RetrieveMatchingTransaction(transaction);

            return maybeTransaction switch
            {
                null => await this.transactionService.CreateTransactionAsync(transaction),
                _ => await this.transactionService.ModifyTransactionAsync(transaction)
            };
        });

        public ValueTask<bool> EnsureTransactionExistAsync(Transaction transaction) =>
        TryCatch(async () =>
        {
            ValidateTransaction(transaction);
            var allUsers = transactionService.RetrieveAllTransactions().ToList();

            return allUsers.Any(retrievedUser => retrievedUser.Id == transaction.Id);

        });
        public ValueTask<bool> VerifyTransactionsExistAsync() =>
        TryCatch(async () =>
        {
            var allUsers = transactionService.RetrieveAllTransactions().ToList();

            return allUsers.Any();

        });

        private Transaction RetrieveMatchingTransaction(Transaction Transaction)
        {
            IQueryable<Transaction> Transactions =
                this.transactionService.RetrieveAllTransactions();

            return Transactions.FirstOrDefault(SameTransactionAs(Transaction));
        }

        private static Expression<Func<Transaction, bool>> SameTransactionAs(Transaction Transaction) =>
            retrieveTransaction => retrieveTransaction.Id == Transaction.Id;





    }
}
