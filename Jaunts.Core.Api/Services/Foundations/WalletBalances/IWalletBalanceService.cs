// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Models.Services.Foundations.WalletBalances;

namespace Jaunts.Core.Api.Services.Foundations.WalletBalances
{
    public interface IWalletBalanceService 
    {
        ValueTask<WalletBalance> CreateWalletBalanceAsync(WalletBalance wallet);
        IQueryable<WalletBalance> RetrieveAllWalletBalances();
        ValueTask<WalletBalance> RetrieveWalletBalanceByIdAsync(Guid walletId);
        ValueTask<WalletBalance> ModifyWalletBalanceAsync(WalletBalance wallet);
        ValueTask<WalletBalance> RemoveWalletBalanceByIdAsync(Guid walletId);
    }
}