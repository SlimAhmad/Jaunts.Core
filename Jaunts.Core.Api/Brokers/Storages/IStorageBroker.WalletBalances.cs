using Jaunts.Core.Api.Models.Services.Foundations.WalletBalances;

namespace Jaunts.Core.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<WalletBalance> InsertWalletBalanceAsync(
            WalletBalance walletBalance);

        IQueryable<WalletBalance> SelectAllWalletBalances();

        ValueTask<WalletBalance> SelectWalletBalanceByIdAsync(
            Guid walletBalanceId);

        ValueTask<WalletBalance> UpdateWalletBalanceAsync(
            WalletBalance walletBalance);

        ValueTask<WalletBalance> DeleteWalletBalanceAsync(
            WalletBalance walletBalance);
    }
}
