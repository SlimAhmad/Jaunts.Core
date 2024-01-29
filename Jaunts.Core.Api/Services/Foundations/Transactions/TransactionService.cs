// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Brokers.DateTimes;
using Jaunts.Core.Api.Brokers.Loggings;
using Jaunts.Core.Api.Brokers.Storages;
using Jaunts.Core.Api.Models.Services.Foundations.Transactions;

namespace Jaunts.Core.Api.Services.Foundations.Transactions
{
    public partial class TransactionService : ITransactionService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public TransactionService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<Transaction> CreateTransactionAsync(Transaction transaction) =>
        TryCatch(async () =>
        {
            ValidateTransactionOnRegister(transaction);

            return await this.storageBroker.InsertTransactionAsync(transaction);
        });

        public IQueryable<Transaction> RetrieveAllTransactions() =>
        TryCatch(() => this.storageBroker.SelectAllTransactions());

        public ValueTask<Transaction> RetrieveTransactionByIdAsync(Guid transactionId) =>
        TryCatch(async () =>
        {
            ValidateTransactionId(transactionId);
            Transaction maybeTransaction = await this.storageBroker.SelectTransactionByIdAsync(transactionId);
            ValidateStorageTransaction(maybeTransaction, transactionId);

            return maybeTransaction;
        });

        public ValueTask<Transaction> ModifyTransactionAsync(Transaction transaction) =>
        TryCatch(async () =>
        {
            ValidateTransactionOnModify(transaction);

            Transaction maybeTransaction =
                await this.storageBroker.SelectTransactionByIdAsync(transaction.Id);

            ValidateStorageTransaction(maybeTransaction, transaction.Id);
            ValidateAgainstStorageTransactionOnModify(inputTransaction: transaction, storageTransaction: maybeTransaction);

            return await this.storageBroker.UpdateTransactionAsync(transaction);
        });

        public ValueTask<Transaction> RemoveTransactionByIdAsync(Guid transactionId) =>
        TryCatch(async () =>
        {
            ValidateTransactionId(transactionId);

            Transaction maybeTransaction =
                await this.storageBroker.SelectTransactionByIdAsync(transactionId);

            ValidateStorageTransaction(maybeTransaction, transactionId);

            return await this.storageBroker.DeleteTransactionAsync(maybeTransaction);
        });

    }
}
