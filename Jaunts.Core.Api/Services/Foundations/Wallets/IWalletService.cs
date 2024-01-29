// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Models.Services.Foundations.Wallets;

namespace Jaunts.Core.Api.Services.Foundations.Wallets
{
    public interface IWalletService 
    {
        ValueTask<Wallet> CreateWalletAsync(Wallet wallet);
        IQueryable<Wallet> RetrieveAllWallets();
        ValueTask<Wallet> RetrieveWalletByIdAsync(Guid walletId);
        ValueTask<Wallet> ModifyWalletAsync(Wallet wallet);
        ValueTask<Wallet> RemoveWalletByIdAsync(Guid walletId);
    }
}