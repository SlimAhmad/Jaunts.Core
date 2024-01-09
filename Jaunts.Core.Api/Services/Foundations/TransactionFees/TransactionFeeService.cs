// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Brokers.DateTimes;
using Jaunts.Core.Api.Brokers.Loggings;
using Jaunts.Core.Api.Brokers.Storages;
using Jaunts.Core.Api.Models.Services.Foundations.TransactionFees;

namespace Jaunts.Core.Api.Services.Foundations.TransactionFees
{
    public partial class TransactionFeeService : ITransactionFeeService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public TransactionFeeService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<TransactionFee> RegisterTransactionFeeAsync(TransactionFee shortlet) =>
        TryCatch(async () =>
        {
            ValidateTransactionFeeOnRegister(shortlet);

            return await this.storageBroker.InsertTransactionFeeAsync(shortlet);
        });

        public IQueryable<TransactionFee> RetrieveAllTransactionFees() =>
        TryCatch(() => this.storageBroker.SelectAllTransactionFees());

        public ValueTask<TransactionFee> RetrieveTransactionFeeByIdAsync(Guid shortletId) =>
        TryCatch(async () =>
        {
            ValidateTransactionFeeId(shortletId);
            TransactionFee maybeTransactionFee = await this.storageBroker.SelectTransactionFeeByIdAsync(shortletId);
            ValidateStorageTransactionFee(maybeTransactionFee, shortletId);

            return maybeTransactionFee;
        });

        public ValueTask<TransactionFee> ModifyTransactionFeeAsync(TransactionFee shortlet) =>
        TryCatch(async () =>
        {
            ValidateTransactionFeeOnModify(shortlet);

            TransactionFee maybeTransactionFee =
                await this.storageBroker.SelectTransactionFeeByIdAsync(shortlet.Id);

            ValidateStorageTransactionFee(maybeTransactionFee, shortlet.Id);
            ValidateAgainstStorageTransactionFeeOnModify(inputTransactionFee: shortlet, storageTransactionFee: maybeTransactionFee);

            return await this.storageBroker.UpdateTransactionFeeAsync(shortlet);
        });

        public ValueTask<TransactionFee> RemoveTransactionFeeByIdAsync(Guid shortletId) =>
        TryCatch(async () =>
        {
            ValidateTransactionFeeId(shortletId);

            TransactionFee maybeTransactionFee =
                await this.storageBroker.SelectTransactionFeeByIdAsync(shortletId);

            ValidateStorageTransactionFee(maybeTransactionFee, shortletId);

            return await this.storageBroker.DeleteTransactionFeeAsync(maybeTransactionFee);
        });

    }
}
