using Jaunts.Core.Api.Models.Services.Foundations.Wallets;

namespace Jaunts.Core.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<Wallet> InsertWalletAsync(
            Wallet wallet);

        IQueryable<Wallet> SelectAllWallets();

        ValueTask<Wallet> SelectWalletByIdAsync(
            Guid walletId);

        ValueTask<Wallet> UpdateWalletAsync(
            Wallet wallet);

        ValueTask<Wallet> DeleteWalletAsync(
            Wallet wallet);
    }
}
