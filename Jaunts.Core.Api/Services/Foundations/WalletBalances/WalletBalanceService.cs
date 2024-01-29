// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Brokers.DateTimes;
using Jaunts.Core.Api.Brokers.Loggings;
using Jaunts.Core.Api.Brokers.Storages;
using Jaunts.Core.Api.Models.Services.Foundations.WalletBalances;

namespace Jaunts.Core.Api.Services.Foundations.WalletBalances
{
    public partial class WalletBalanceService : IWalletBalanceService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public WalletBalanceService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<WalletBalance> CreateWalletBalanceAsync(WalletBalance walletBalance) =>
        TryCatch(async () =>
        {
            ValidateWalletBalanceOnCreate(walletBalance);

            return await this.storageBroker.InsertWalletBalanceAsync(walletBalance);
        });

        public IQueryable<WalletBalance> RetrieveAllWalletBalances() =>
        TryCatch(() => this.storageBroker.SelectAllWalletBalances());

        public ValueTask<WalletBalance> RetrieveWalletBalanceByIdAsync(Guid walletBalanceId) =>
        TryCatch(async () =>
        {
            ValidateWalletBalanceId(walletBalanceId);
            WalletBalance maybeWalletBalance = await this.storageBroker.SelectWalletBalanceByIdAsync(walletBalanceId);
            ValidateStorageWalletBalance(maybeWalletBalance, walletBalanceId);

            return maybeWalletBalance;
        });

        public ValueTask<WalletBalance> ModifyWalletBalanceAsync(WalletBalance walletBalance) =>
        TryCatch(async () =>
        {
            ValidateWalletBalanceOnModify(walletBalance);

            WalletBalance maybeWalletBalance =
                await this.storageBroker.SelectWalletBalanceByIdAsync(walletBalance.Id);

            ValidateStorageWalletBalance(maybeWalletBalance, walletBalance.Id);
            ValidateAgainstStorageWalletBalanceOnModify(inputWalletBalance: walletBalance, storageWalletBalance: maybeWalletBalance);

            return await this.storageBroker.UpdateWalletBalanceAsync(walletBalance);
        });

        public ValueTask<WalletBalance> RemoveWalletBalanceByIdAsync(Guid walletBalanceId) =>
        TryCatch(async () =>
        {
            ValidateWalletBalanceId(walletBalanceId);

            WalletBalance maybeWalletBalance =
                await this.storageBroker.SelectWalletBalanceByIdAsync(walletBalanceId);

            ValidateStorageWalletBalance(maybeWalletBalance, walletBalanceId);

            return await this.storageBroker.DeleteWalletBalanceAsync(maybeWalletBalance);
        });

    }
}
