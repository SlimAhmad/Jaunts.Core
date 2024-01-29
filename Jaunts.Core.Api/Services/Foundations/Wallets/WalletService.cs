// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Brokers.DateTimes;
using Jaunts.Core.Api.Brokers.Loggings;
using Jaunts.Core.Api.Brokers.Storages;
using Jaunts.Core.Api.Models.Services.Foundations.Wallets;

namespace Jaunts.Core.Api.Services.Foundations.Wallets
{
    public partial class WalletService : IWalletService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public WalletService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<Wallet> CreateWalletAsync(Wallet wallet) =>
        TryCatch(async () =>
        {
            ValidateWalletOnCreate(wallet);

            return await this.storageBroker.InsertWalletAsync(wallet);
        });

        public IQueryable<Wallet> RetrieveAllWallets() =>
        TryCatch(() => this.storageBroker.SelectAllWallets());

        public ValueTask<Wallet> RetrieveWalletByIdAsync(Guid walletId) =>
        TryCatch(async () =>
        {
            ValidateWalletId(walletId);
            Wallet maybeWallet = await this.storageBroker.SelectWalletByIdAsync(walletId);
            ValidateStorageWallet(maybeWallet, walletId);

            return maybeWallet;
        });

        public ValueTask<Wallet> ModifyWalletAsync(Wallet wallet) =>
        TryCatch(async () =>
        {
            ValidateWalletOnModify(wallet);

            Wallet maybeWallet =
                await this.storageBroker.SelectWalletByIdAsync(wallet.Id);

            ValidateStorageWallet(maybeWallet, wallet.Id);
            ValidateAgainstStorageWalletOnModify(inputWallet: wallet, storageWallet: maybeWallet);

            return await this.storageBroker.UpdateWalletAsync(wallet);
        });

        public ValueTask<Wallet> RemoveWalletByIdAsync(Guid walletId) =>
        TryCatch(async () =>
        {
            ValidateWalletId(walletId);

            Wallet maybeWallet =
                await this.storageBroker.SelectWalletByIdAsync(walletId);

            ValidateStorageWallet(maybeWallet, walletId);

            return await this.storageBroker.DeleteWalletAsync(maybeWallet);
        });

    }
}
