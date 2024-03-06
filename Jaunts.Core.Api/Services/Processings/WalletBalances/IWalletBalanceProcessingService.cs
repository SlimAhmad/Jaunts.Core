using Jaunts.Core.Api.Models.Services.Foundations.WalletBalances;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Models.Email;

namespace Jaunts.Core.Api.Services.Processings.WalletBalances
{
    public partial interface IWalletBalanceProcessingService
    {
        IQueryable<WalletBalance> RetrieveAllWalletBalances();
        ValueTask<bool> RemoveWalletBalanceByIdAsync(Guid id);
        ValueTask<WalletBalance> RetrieveWalletBalanceByIdAsync(Guid id);
        ValueTask<WalletBalance> UpsertWalletBalanceAsync(WalletBalance walletBalance);

    }
}
