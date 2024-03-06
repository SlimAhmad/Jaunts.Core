using Jaunts.Core.Api.Brokers.Loggings;
using Jaunts.Core.Api.Models.Services.Foundations.WalletBalances;
using Jaunts.Core.Api.Services.Foundations.WalletBalances;
using System.Linq.Expressions;

namespace Jaunts.Core.Api.Services.Processings.WalletBalances
{
    public partial class WalletBalanceProcessingService : IWalletBalanceProcessingService
    {
      
            private readonly IWalletBalanceService  walletBalanceService;
            private readonly ILoggingBroker loggingBroker;

            public WalletBalanceProcessingService(
                IWalletBalanceService  walletBalanceService,
                ILoggingBroker loggingBroker

                )
            {
                this.walletBalanceService = walletBalanceService;
                this.loggingBroker = loggingBroker;

            }

           public IQueryable<WalletBalance> RetrieveAllWalletBalances() =>
           TryCatch(() => this.walletBalanceService.RetrieveAllWalletBalances());
           public ValueTask<bool> RemoveWalletBalanceByIdAsync(
                Guid id) =>
            TryCatch(async () =>
            {
                ValidateWalletBalanceId(id);
                var userWalletBalance = await walletBalanceService.RemoveWalletBalanceByIdAsync(id);
                ValidateWalletBalance(userWalletBalance);
                return true;
            });

            public ValueTask<WalletBalance> RetrieveWalletBalanceByIdAsync(Guid id) =>
            TryCatch(async () =>
            {
                ValidateWalletBalanceId(id);
                var wallet = await walletBalanceService.RetrieveWalletBalanceByIdAsync(id);
                ValidateWalletBalance(wallet);
                return wallet;
            });

            public ValueTask<WalletBalance> UpsertWalletBalanceAsync(
                    WalletBalance  walletBalance) =>
                TryCatch(async () =>
                {
                    ValidateWalletBalance(walletBalance);
                    WalletBalance maybeWalletBalance = RetrieveMatchingWalletBalance(walletBalance);

                    return maybeWalletBalance switch
                    {
                        null => await this.walletBalanceService.CreateWalletBalanceAsync(walletBalance),
                        _ => await this.walletBalanceService.ModifyWalletBalanceAsync(walletBalance)
                    };
                });




            private WalletBalance RetrieveMatchingWalletBalance(WalletBalance walletBalance)
            {
                IQueryable<WalletBalance> walletBalances =
                    this.walletBalanceService.RetrieveAllWalletBalances();

                return walletBalances.FirstOrDefault(SameWalletBalanceAs(walletBalance));
            }

            private static Expression<Func<WalletBalance, bool>> SameWalletBalanceAs(WalletBalance walletBalance) =>
                retrieveWalletBalance => retrieveWalletBalance.Id == walletBalance.Id;





    }
}
