using Jaunts.Core.Api.Models.Services.Foundations.Wallets;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Models.Email;

namespace Jaunts.Core.Api.Services.Processings.Wallets
{
    public partial interface IWalletProcessingService
    {
        IQueryable<Wallet> RetrieveAllWallets();
        ValueTask<bool> RemoveWalletByIdAsync(Guid id);
        ValueTask<Wallet> RetrieveWalletByIdAsync(Guid id);
        ValueTask<Wallet> UpsertWalletAsync(Wallet Wallet);

    }
}
